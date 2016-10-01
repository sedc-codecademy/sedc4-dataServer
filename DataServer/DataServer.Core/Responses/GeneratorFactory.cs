using DataServer.Requests;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DataServer.Responses
{
    public class GeneratorFactory
    {
        private Regex routePrefixRegex = new Regex(@"^\/([^\/]*)\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private Dictionary<string, IResponseGenerator> registeredGenerators = new Dictionary<string, IResponseGenerator>();

        public virtual IResponseGenerator GetResponseGenerator(RequestData request)
        {
            var routePrefix = GetRoutePrefix(request.Location);
            if (registeredGenerators.ContainsKey(routePrefix))
                return registeredGenerators[routePrefix];

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

        private string GetRoutePrefix(string location)
        {
            var match = routePrefixRegex.Match(location);
            if (!match.Success)
                return string.Empty;
            return match.Groups[1].Value;
        }

        public void RegisterGenerator<T>(string routePrefix) where T : IResponseGenerator, new()
        {
            if (registeredGenerators.ContainsKey(routePrefix))
                registeredGenerators[routePrefix] = new T();
            else
                registeredGenerators.Add(routePrefix, new T());
        }

        public bool IsRouteRegistered(string routePrefix)
        {
            return registeredGenerators.ContainsKey(routePrefix);
        }

        private bool IsJsonRequest(RequestData request)
        {
            return false;
        }

        private bool IsFileRequest(RequestData request)
        {
            if (request.Method != Method.Get)
                return false;

            var filePattern = new Regex(@"^\/[a-z0-9]+\.[a-z0-9]+$", RegexOptions.IgnoreCase);

            return filePattern.IsMatch(request.Location);
        }
    }
}