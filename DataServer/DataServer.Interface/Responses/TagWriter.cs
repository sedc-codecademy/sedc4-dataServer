using System;

namespace DataServer.Responses
{
    public class TagWriter
    {
        public static string Create(string tagName, string content)
        {
            return $"<{tagName}>{content}</{tagName}>";
        }
    }
}