using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Exceptions;
using ExperBE.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace ExperBE.Tests.Middleware
{
    [TestClass]
    public class CustomExceptionMiddlewareTests
    {
        [TestMethod]
        public async Task CustomExceptionMiddleware_IfBadRequestException_SetsDataAndWritesJson()
        {
            var middleware = new CustomExceptionMiddleware();
            var context = new DefaultHttpContext();
            await using var ms = new MemoryStream();
            context.Response.Body = ms;
            var requestDelegate = new Mock<RequestDelegate>();
            requestDelegate.Setup(r => r.Invoke(It.IsAny<HttpContext>())).Throws(new BadRequestException("property", "errorMessage"));
            await middleware.InvokeAsync(context, requestDelegate.Object);
            Assert.AreEqual("application/json", context.Response.ContentType);
            Assert.AreEqual(400, context.Response.StatusCode);

            ms.Position = 0;
            var streamReader = new StreamReader(ms);
            var body = JsonConvert.DeserializeObject<BadRequestExceptionError>(await streamReader.ReadToEndAsync());
            Assert.AreEqual("property", body.Errors.First().Key);
            Assert.AreEqual("errorMessage", body.Errors.First().Value.First());
        }

        [TestMethod]
        public async Task CustomExceptionMiddleware_IfNoException_Continues()
        {
            var middleware = new CustomExceptionMiddleware();
            var context = new DefaultHttpContext();
            var requestDelegate = new Mock<RequestDelegate>();
            requestDelegate.Setup(r => r.Invoke(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);
            await middleware.InvokeAsync(context, requestDelegate.Object);
            Assert.AreNotEqual(400, context.Response.StatusCode);
        }
    }
}
