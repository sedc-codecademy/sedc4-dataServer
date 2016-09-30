using System;

namespace DemoServer
{
    internal class TagWriter
    {
        internal static string Create(string tagName, string content)
        {
            return $"<{tagName}>{content}</{tagName}>";
        }
    }
}