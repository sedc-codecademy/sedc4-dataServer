using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace DemoServer
{
    public class RequestParser
    {
        public RequestParser()
        {
        }

        public RequestData ParseRequest(string request)
        {
            if (request == string.Empty)
                return new RequestData();

            var result = new RequestData();

            var lines = request.Split('\n').Select(l => l.Trim()).ToList();
            var firstLine = lines[0];

            var firstLineParts = firstLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            result.Method = GetMethod(firstLineParts[0]);

            var fullLocation = firstLineParts[1];
            var querySeparatorIndex = fullLocation.IndexOf("?");
            if (querySeparatorIndex == -1)
            {
                result.Location = fullLocation;
            }
            else
            {
                result.Location = fullLocation.Substring(0, querySeparatorIndex);
                var query = fullLocation.Substring(querySeparatorIndex + 1);
                result.QueryParameters = GetQueryParameters(query);
            }

            var headerLines = lines.Skip(1).TakeWhile(l => l != string.Empty);
            foreach (var line in headerLines)
            {
                var lineParts = line.IndexOf(':');
                var header = line.Substring(0, lineParts);
                var value = line.Substring(lineParts + 1).Trim();
                result.Headers.Add(header, value);
            }

            var beforeBodyLines = 1 + headerLines.Count() + 1;
            result.Body = string.Join("\n", lines.Skip(beforeBodyLines));

            return result;
        }

        private static Dictionary<string, string> GetQueryParameters(string query)
        {
            var result = new Dictionary<string, string>();
            var queryItems = query.Split('&');
            foreach (var queryItem in queryItems)
            {
                var queryItemParts = queryItem.Split('=');
                var value = queryItemParts[1];
                value = UrlDecode(value);
                result.Add(queryItemParts[0], value);
            }
            return result;
        }

        private static string UrlDecode(string value)
        {
            return WebUtility.UrlDecode(value);
        }

        private static Dictionary<string, Method> methodResolver = new Dictionary<string, Method>
        {
            { "get",  Method.Get },
            { "post",  Method.Post },
            { "delete",  Method.Delete },
            { "put",  Method.Put },
            { "fetch",  Method.Fetch },
            { "options",  Method.Options },
        };
        private static Method GetMethod(string methodString)
        {
            methodString = methodString.ToLower();
            if (methodResolver.ContainsKey(methodString))
                return methodResolver[methodString];
            return Method.Unknown;
        }
    }
}