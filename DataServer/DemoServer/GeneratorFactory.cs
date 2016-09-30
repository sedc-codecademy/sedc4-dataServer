using System;
using System.Text.RegularExpressions;

namespace DemoServer
{
    internal static class GeneratorFactory
    {
        public static IResponseGenerator GetResponseGenerator(RequestData request)
        {
            if (IsFileRequest(request))
            {
                return new FileResponseGenerator();
            }
            else if (IsJsonRequest(request))
            {
                return new JsonResponseGenerator();
            }
            else
            {
                return new StringResponseGenerator();
            }
        }

        private static bool IsJsonRequest(RequestData request)
        {
            return false;
        }

        private static bool IsFileRequest(RequestData request)
        {
            if (request.Method != Method.Get)
                return false;

            var filePattern = new Regex(@"^\/[a-z0-9]+\.[a-z0-9]+$", RegexOptions.IgnoreCase);

            return filePattern.IsMatch(request.Location);
        }
    }
}