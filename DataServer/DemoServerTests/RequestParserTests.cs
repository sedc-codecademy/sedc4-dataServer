using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoServer.Tests
{
    [TestClass]
    public class RequestParserTests
    {
        [TestMethod]
        public void ParseRequest_EmptyRequest_Test()
        {
            //1. Arrange
            var parser = new RequestParser();
            var request = string.Empty;
            //2. Act
            var actual = parser.ParseRequest(request);
            //3. Assert
            Assert.AreEqual(Method.Unknown, actual.Method);
            Assert.AreEqual(0, actual.Headers.Count());
        }

        [TestMethod]
        public void ParseRequest_BasicRequest_Test()
        {
            //1. Arrange
            var parser = new RequestParser();
            var request = @"GET / HTTP/1.1
Host: localhost:8082
Connection: keep-alive
User-Agent: Browser-Specific
Accept: text/html";
            //2. Act
            var actual = parser.ParseRequest(request);
            //3. Assert
            Assert.AreEqual(Method.Get, actual.Method);
            Assert.AreEqual(4, actual.Headers.Count());
            var host = actual.Headers.Get("host");
            Assert.AreEqual("localhost:8082", host.Value);
            var connection = actual.Headers.Get("connection");
            Assert.AreEqual("keep-alive", connection.Value);
            var ua = actual.Headers.Get("User-Agent");
            Assert.AreEqual("Browser-Specific", ua.Value);
            var accept = actual.Headers.Get("Accept");
            Assert.AreEqual("text/html", accept.Value);
        }
    }
}