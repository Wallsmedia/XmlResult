// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Internal;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Test.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions
{

    public class XmlResultTest
    {
        [Fact]
        public async Task ExecuteAsync_WritesXmlContent()
        {
            // Arrange
            var value = new PurchaseOrder();
            var xmlWriterSettings = FormattingUtilities.GetDefaultXmlWriterSettings();
            xmlWriterSettings.CloseOutput = false;
            var textw = new StringWriter();
            var writer = XmlWriter.Create(textw, xmlWriterSettings);
            var xmlSerializer = new XmlSerializer(value.GetType());
            xmlSerializer.Serialize(writer, value);
            var expected = Encoding.UTF8.GetBytes(textw.ToString());
            var context = GetActionContext();
            CreateServices(context.HttpContext);

            //
            var result = new XmlResult(value);

            // Act
            await result.ExecuteResultAsync(context);

            // Assert
            var written = GetWrittenBytes(context.HttpContext);

            var s1 = Encoding.UTF8.GetString(expected);
            var s2 = Encoding.UTF8.GetString(written);

            //Assert.AreEqual(expected, written);
            Assert.Equal(s1, s2);
            Assert.Equal("application/xml; charset=utf-8", context.HttpContext.Response.ContentType);
        }
        [Fact]
        public async Task ExecuteAsync_WritesXmlContent_Negative()
        {
            // Arrange
            var value = new PurchaseOrder();
            var xmlWriterSettings = FormattingUtilities.GetDefaultXmlWriterSettings();
            xmlWriterSettings.CloseOutput = false;
            var textw = new StringWriter();
            var writer = XmlWriter.Create(textw, xmlWriterSettings);
            var xmlSerializer = new XmlSerializer(value.GetType());
            xmlSerializer.Serialize(writer, value);
            var expected = Encoding.UTF8.GetBytes(textw.ToString());
            var context = GetActionContext();
            CreateServices(context.HttpContext, true);

            //
            var result = new XmlResult(value);

            // Act
            await result.ExecuteResultAsync(context);

            // Assert
            Assert.Equal(context.HttpContext?.Response.StatusCode, StatusCodes.Status406NotAcceptable);
        }

        [Fact]
        public async Task ExecuteAsync_WritesXmlDataContractContent()
        {
            // Arrange
            var value = new PurchaseOrder();
            var context = GetActionContext();
            CreateServices(context.HttpContext);

            //
            var result = new XmlResult(value) { XmlSerializerType = XmlSerializerType.DataContractSerializer };

            // Act
            await result.ExecuteResultAsync(context);

            // Assert
            Assert.Equal("application/xml; charset=utf-8", context.HttpContext.Response.ContentType);

            // Verify to as the new restored object 
            //There may be differ DataContract style has been used
            var written = GetWrittenBytes(context.HttpContext);
            var sWritten = Encoding.UTF8.GetString(written);

            StringReader sreader = new StringReader(sWritten);
            DataContractSerializer ser = new DataContractSerializer(typeof(PurchaseOrder));
            PurchaseOrder? newValue = (PurchaseOrder)ser.ReadObject(XmlReader.Create(sreader))!;

            Assert.Equal(value.billTo.street, newValue?.billTo?.street);
            Assert.Equal(value.shipTo.street, newValue?.shipTo?.street);
        }

        [Fact]
        public async Task ExecuteAsync_WritesXmlDataContractContent_Negative()
        {
            // Arrange
            var value = new PurchaseOrder();
            var context = GetActionContext();
            CreateServices(context.HttpContext, true);

            //
            var result = new XmlResult(value) { XmlSerializerType = XmlSerializerType.DataContractSerializer };

            // Act
            await result.ExecuteResultAsync(context);

            Assert.Equal(context.HttpContext?.Response.StatusCode, StatusCodes.Status406NotAcceptable);
        }

        private static HttpContext CreateServices(HttpContext httpContext, bool empty = false)
        {
            IHttpResponseStreamWriterFactory writerFactory = new TestHttpResponseStreamWriterFactory();
            ILoggerFactory loggerFactory = NullLoggerFactory.Instance;

            var services = new ServiceCollection();

            services.AddSingleton(writerFactory);
            services.AddSingleton(loggerFactory);
            services.AddSingleton(Options.Create(new MvcOptions()));

            if (!empty)
            {
                var executorXml = new XmlResultExecutor(writerFactory, loggerFactory);
                var executorDcXml = new XmlDcResultExecutor(writerFactory, loggerFactory);
                services.AddSingleton(executorXml);
                services.AddSingleton(executorDcXml);
            }
            httpContext.RequestServices = services.BuildServiceProvider();
            return httpContext;
        }

        private static HttpContext GetHttpContext(string contentType = "application/xml; charset=utf-8")
        {
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Headers["Accept-Charset"] = MediaTypeHeaderValue.Parse(contentType).Charset.ToString();
            request.ContentType = contentType;
            httpContext.Response.Body = new MemoryStream();
            httpContext.RequestServices = new ServiceCollection()
                .AddSingleton(Options.Create(new MvcOptions()))
                .BuildServiceProvider();
            return httpContext;
        }


        private static ActionContext GetActionContext()
        {
            return new ActionContext(GetHttpContext(), new RouteData(), new ActionDescriptor());
        }

        private static byte[] GetWrittenBytes(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            return ((MemoryStream)(context.Response.Body)).ToArray();
        }
    }
}