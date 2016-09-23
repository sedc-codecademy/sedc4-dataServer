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
            Assert.AreEqual(Method.Get, actual.Method);
            Assert.AreEqual(0, actual.Headers.Count());
        }
    }
}