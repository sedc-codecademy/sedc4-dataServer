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
            Assert.AreEqual(string.Empty, actual.Body);
            Assert.AreEqual(string.Empty, actual.Location);
            Assert.AreEqual(0, actual.QueryParameters.Count);
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
            Assert.AreEqual(string.Empty, actual.Body);
            Assert.AreEqual("/", actual.Location);
            Assert.AreEqual(0, actual.QueryParameters.Count);
        }

        [TestMethod]
        public void ParseRequest_PostRequest_WithBody_WithPath_WithQuery_Test()
        {
            //1. Arrange
            var parser = new RequestParser();
            var request = @"POST /path/to/somewhere/file.ext?two=2&one=1&whatever=something%20a HTTP/1.1
Host: localhost:8082
Connection: keep-alive
Content-Length: 231
my-header: my-value, my-other-vaklue

------WebKitFormBoundaryJhEgYLCH0XPdvLDn
Content-Disposition: form-data; name=""name""

weko
------WebKitFormBoundaryJhEgYLCH0XPdvLDn
Content - Disposition: form - data; name = ""age""

39
------WebKitFormBoundaryJhEgYLCH0XPdvLDn--";
            //2. Act
            var actual = parser.ParseRequest(request);
            //3. Assert
            Assert.AreEqual(Method.Post, actual.Method);
            Assert.AreEqual(4, actual.Headers.Count());
            var host = actual.Headers.Get("host");
            Assert.AreEqual("localhost:8082", host.Value);
            var connection = actual.Headers.Get("connection");
            Assert.AreEqual("keep-alive", connection.Value);
            var cl = actual.Headers.Get("Content-Length");
            Assert.AreEqual("231", cl.Value);
            var myHeader = actual.Headers.Get("my-header");
            Assert.AreEqual("my-value, my-other-vaklue", myHeader.Value);
            Assert.AreEqual(@"------WebKitFormBoundaryJhEgYLCH0XPdvLDn
Content-Disposition: form-data; name=""name""

weko
------WebKitFormBoundaryJhEgYLCH0XPdvLDn
Content - Disposition: form - data; name = ""age""

39
------WebKitFormBoundaryJhEgYLCH0XPdvLDn--", actual.Body);
            Assert.AreEqual("/path/to/somewhere/file.ext", actual.Location);
            Assert.AreEqual(3, actual.QueryParameters.Count);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("one"));
            Assert.AreEqual("1", actual.QueryParameters["one"]);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("two"));
            Assert.AreEqual("2", actual.QueryParameters["two"]);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("whatever"));
            Assert.AreEqual("something a", actual.QueryParameters["whatever"]);
        }

        [TestMethod]
        public void ParseRequest_PostRequest_WithBody_WithPath_NoQuery_Test()
        {
            //1. Arrange
            var parser = new RequestParser();
            var request = @"POST /path/to/somewhere/file.ext HTTP/1.1
Host: localhost:8082
Connection: keep-alive
Content-Length: 231
my-header: my-value, my-other-vaklue

------WebKitFormBoundaryJhEgYLCH0XPdvLDn
Content-Disposition: form-data; name=""name""

weko
------WebKitFormBoundaryJhEgYLCH0XPdvLDn
Content - Disposition: form - data; name = ""age""

39
------WebKitFormBoundaryJhEgYLCH0XPdvLDn--";
            //2. Act
            var actual = parser.ParseRequest(request);
            //3. Assert
            Assert.AreEqual(Method.Post, actual.Method);
            Assert.AreEqual(4, actual.Headers.Count());
            var host = actual.Headers.Get("host");
            Assert.AreEqual("localhost:8082", host.Value);
            var connection = actual.Headers.Get("connection");
            Assert.AreEqual("keep-alive", connection.Value);
            var cl = actual.Headers.Get("Content-Length");
            Assert.AreEqual("231", cl.Value);
            var myHeader = actual.Headers.Get("my-header");
            Assert.AreEqual("my-value, my-other-vaklue", myHeader.Value);
            Assert.AreEqual(@"------WebKitFormBoundaryJhEgYLCH0XPdvLDn
Content-Disposition: form-data; name=""name""

weko
------WebKitFormBoundaryJhEgYLCH0XPdvLDn
Content - Disposition: form - data; name = ""age""

39
------WebKitFormBoundaryJhEgYLCH0XPdvLDn--", actual.Body);
            Assert.AreEqual("/path/to/somewhere/file.ext", actual.Location);
            Assert.AreEqual(0, actual.QueryParameters.Count);
        }

        [TestMethod]
        public void ParseRequest_PostRequest_NoBody_WithPath_WithQuery_Test()
        {
            //1. Arrange
            var parser = new RequestParser();
            var request = @"POST /path/to/somewhere/file.ext?two=2&one=1&whatever=something%20a HTTP/1.1
Host: localhost:8082
Connection: keep-alive
Content-Length: 231
my-header: my-value, my-other-vaklue";
            //2. Act
            var actual = parser.ParseRequest(request);
            //3. Assert
            Assert.AreEqual(Method.Post, actual.Method);
            Assert.AreEqual(4, actual.Headers.Count());
            var host = actual.Headers.Get("host");
            Assert.AreEqual("localhost:8082", host.Value);
            var connection = actual.Headers.Get("connection");
            Assert.AreEqual("keep-alive", connection.Value);
            var cl = actual.Headers.Get("Content-Length");
            Assert.AreEqual("231", cl.Value);
            var myHeader = actual.Headers.Get("my-header");
            Assert.AreEqual("my-value, my-other-vaklue", myHeader.Value);
            Assert.AreEqual(string.Empty, actual.Body);
            Assert.AreEqual("/path/to/somewhere/file.ext", actual.Location);
            Assert.AreEqual(3, actual.QueryParameters.Count);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("one"));
            Assert.AreEqual("1", actual.QueryParameters["one"]);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("two"));
            Assert.AreEqual("2", actual.QueryParameters["two"]);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("whatever"));
            Assert.AreEqual("something a", actual.QueryParameters["whatever"]);
        }

        [TestMethod]
        public void ParseRequest_DeleteRequest_NoBody_NoPath_WithQuery_Test()
        {
            //1. Arrange
            var parser = new RequestParser();
            var request = @"DELETE /?two=2&one=1&whatever=something%20a HTTP/1.1
Host: localhost:8082
Connection: keep-alive
Content-Length: 231
my-header: my-value, my-other-vaklue";
            //2. Act
            var actual = parser.ParseRequest(request);
            //3. Assert
            Assert.AreEqual(Method.Delete, actual.Method);
            Assert.AreEqual(4, actual.Headers.Count());
            var host = actual.Headers.Get("host");
            Assert.AreEqual("localhost:8082", host.Value);
            var connection = actual.Headers.Get("connection");
            Assert.AreEqual("keep-alive", connection.Value);
            var cl = actual.Headers.Get("Content-Length");
            Assert.AreEqual("231", cl.Value);
            var myHeader = actual.Headers.Get("my-header");
            Assert.AreEqual("my-value, my-other-vaklue", myHeader.Value);
            Assert.AreEqual(string.Empty, actual.Body);
            Assert.AreEqual("/", actual.Location);
            Assert.AreEqual(3, actual.QueryParameters.Count);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("one"));
            Assert.AreEqual("1", actual.QueryParameters["one"]);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("two"));
            Assert.AreEqual("2", actual.QueryParameters["two"]);
            Assert.IsTrue(actual.QueryParameters.ContainsKey("whatever"));
            Assert.AreEqual("something a", actual.QueryParameters["whatever"]);
        }
    }
}