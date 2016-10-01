using DataServer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServer.Requests;
using DataBrowser.Core;
using System.Text.RegularExpressions;
using DataServer.Entities;
using System.Net;
using Newtonsoft.Json;

namespace DataServer.MsSql
{
    public class MsSqlResponseGenerator : IResponseGenerator
    {
        private Regex fullPathRegex = new Regex(@"^\/mssql\/([^\/]*)\/([^\/]*)\/([^\/]*)\/([^\/]*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public IResponseData GenerateResponse(RequestData request)
        {
            var result = new ResponseData<string>();
            result.Headers.Add("Content-type", "application/json");

            var match = fullPathRegex.Match(request.Location);
            if (!match.Success)
            {
                return new ErrorResponseData(StatusCode.NotImplemented);
            }

            var serverName = WebUtility.UrlDecode(match.Groups[1].Value);
            var databaseName = WebUtility.UrlDecode(match.Groups[2].Value);
            var tableName = WebUtility.UrlDecode(match.Groups[3].Value);
            var action = WebUtility.UrlDecode(match.Groups[4].Value);

            var connectionData = new ConnectionData
            {
                AuthType = AuthenticationType.Windows,
                ServerName = serverName
            };
            var service = new DataBrowseService(connectionData);

            var data = service.GetData(databaseName, tableName);
            var dataJson = JsonConvert.SerializeObject(data);

            result.Payload = dataJson;
            return result;
        }
    }
}
