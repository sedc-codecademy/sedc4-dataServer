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
        public IResponseData GenerateResponse(RequestData request)
        {
            var parsedCommand = CommandParser.ParseCommand(GetPath(request.Location));
            switch (parsedCommand.CommandType)
            {
                case CommandType.TableLevelCommand:
                    return GetTableResponseData(parsedCommand);
                case CommandType.DatabaseLevelCommand:
                    return GetDatabaseTables(parsedCommand);
                case CommandType.ServerLevelCommand:
                    return GetServerDatabases(parsedCommand);
                case CommandType.WrongFormat:
                default:
                    return new ErrorResponseData(StatusCode.NotImplemented);
            }
        }

        private IResponseData GetServerDatabases(ParsedCommand parsedCommand)
        {
            var result = new ResponseData<string>();
            result.Headers.Add("Content-type", "application/json");
            try
            {
                var connectionData = new ConnectionData
                {
                    AuthType = AuthenticationType.Windows,
                    ServerName = parsedCommand.ServerName
                };
                var service = new DataBrowseService(connectionData);
                service.Connect().Wait();
                var data = service.GetDatabaseNames().Result;
                var dataJson = JsonConvert.SerializeObject(data);

                result.Payload = dataJson;
                return result;
            }
            catch (Exception)
            {
                return new ErrorResponseData(StatusCode.BadRequest);
            }
        }

        private IResponseData GetDatabaseTables(ParsedCommand parsedCommand)
        {
            try
            {
                var result = new ResponseData<string>();
                result.Headers.Add("Content-type", "application/json");
                var connectionData = new ConnectionData
                {
                    AuthType = AuthenticationType.Windows,
                    ServerName = parsedCommand.ServerName
                };
                var service = new DataBrowseService(connectionData);
                service.Connect().Wait();
                var data = service.GetTableNames(parsedCommand.DatabaseName).Result;
                var dataJson = JsonConvert.SerializeObject(data);

                result.Payload = dataJson;
                return result;
            }
            catch (Exception)
            {
                return new ErrorResponseData(StatusCode.BadRequest);
            }

        }

        private IResponseData GetTableResponseData(ParsedCommand parsedCommand)
        {
            try
            {
                var result = new ResponseData<string>();
                result.Headers.Add("Content-type", "application/json");
                var connectionData = new ConnectionData
                {
                    AuthType = AuthenticationType.Windows,
                    ServerName = parsedCommand.ServerName
                };
                var service = new DataBrowseService(connectionData);
                service.Connect().Wait();

                //dynamic data;

                //switch (parsedCommand.Action.ToLowerInvariant())
                //{
                //    case "names":
                //        data = service.GetColumnNames(parsedCommand.DatabaseName, parsedCommand.TableName).Result;
                //        break;
                //    case "data":
                //        data = service.GetData(parsedCommand.DatabaseName, parsedCommand.TableName).Result;
                //        break;
                //    default:
                //        return new ErrorResponseData(StatusCode.NotImplemented);
                //}

                string methodName;
                switch (parsedCommand.Action.ToLowerInvariant())
                {
                    case "names":
                        methodName = "GetColumnNames";
                        break;
                    case "data":
                        methodName = "GetData";
                        break;
                    default:
                        return new ErrorResponseData(StatusCode.NotImplemented);
                }

                var serviceType = service.GetType();
                var method = serviceType.GetMethod(methodName);
                var objectResult = method.Invoke(service, new object[] { parsedCommand.DatabaseName, parsedCommand.TableName });

                var objectResultType = objectResult.GetType();
                var resultProperty = objectResultType.GetProperty("Result");
                var data = resultProperty.GetValue(objectResult);

                var dataJson = JsonConvert.SerializeObject(data);

                result.Payload = dataJson;
                return result;
            }
            catch (Exception)
            {
                return new ErrorResponseData(StatusCode.BadRequest);
            }

        }

        private string GetPath(string location)
        {
            var indexOfSlash = location.IndexOf("/", 1);
            var path = location.Substring(indexOfSlash + 1);
            return path;
        }
    }
}
