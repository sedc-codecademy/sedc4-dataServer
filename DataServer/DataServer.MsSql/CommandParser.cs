using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DataServer.MsSql
{
    internal class CommandParser
    {
        internal static ParsedCommand ParseCommand(string command)
        {
            var parts = command.Split('/').Select(p => WebUtility.UrlDecode(p)).ToArray();

            if (parts.Length == 2)
            {
                return new ParsedCommand
                {
                    Command = command,
                    ServerName = parts[0],
                    Action = parts[1],
                    CommandType = CommandType.ServerLevelCommand
                };
            }
            else if (parts.Length == 3)
            {
                return new ParsedCommand
                {
                    Command = command,
                    ServerName = parts[0],
                    DatabaseName = parts[1],
                    Action = parts[2],
                    CommandType = CommandType.DatabaseLevelCommand
                };
            }
            else if (parts.Length == 4)
            {
                return new ParsedCommand
                {
                    Command = command,
                    ServerName = parts[0],
                    DatabaseName = parts[1],
                    TableName = parts[2],
                    Action = parts[3],
                    CommandType = CommandType.TableLevelCommand
                };
            }
            return new ParsedCommand { Command = command};
        }
    }

    public class ParsedCommand
    {
        public string Command { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public CommandType CommandType { get; set; }

        public ParsedCommand()
        {
            CommandType = CommandType.WrongFormat;
        }
    }

    public enum CommandType
    {
        ServerLevelCommand,
        DatabaseLevelCommand,
        TableLevelCommand,
        WrongFormat
    }
}