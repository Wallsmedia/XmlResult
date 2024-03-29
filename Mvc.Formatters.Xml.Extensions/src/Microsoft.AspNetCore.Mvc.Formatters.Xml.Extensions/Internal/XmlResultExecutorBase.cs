// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Internal;

namespace Microsoft.AspNetCore.Mvc.Formatters.Xml.Internal;

/// <summary>
/// Executes a <see cref="XmlResult"/> to write to the response.
/// </summary>
public class XmlResultExecutorBase : IXmlResultExecutor
{
    private static readonly string DefaultContentType = new MediaTypeHeaderValue("application/xml")
    {
        Encoding = Encoding.UTF8
    }.ToString();

    private static readonly Encoding DefaultEncoding = Encoding.UTF8;

    /// <summary>
    /// Creates a new <see cref="XmlResultExecutor"/>.
    /// </summary>
    /// <param name="writerFactory">The <see cref="IHttpResponseStreamWriterFactory"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
    /// <param name="outputFormatter">The <see cref="TextOutputFormatter"/>.</param>
    public XmlResultExecutorBase(
        IHttpResponseStreamWriterFactory writerFactory,
        ILoggerFactory loggerFactory, TextOutputFormatter outputFormatter)
    {
        if (writerFactory == null)
        {
            throw new ArgumentNullException(nameof(writerFactory));
        }

        if (outputFormatter == null)
        {
            throw new ArgumentNullException(nameof(outputFormatter));
        }

        if (loggerFactory == null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        WriterFactory = writerFactory;
        Logger = loggerFactory.CreateLogger<XmlResult>();
        OutputFormatter = outputFormatter;
    }

    TextOutputFormatter OutputFormatter { get; }
    /// <summary>
    /// Gets the <see cref="ILogger"/>.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets the <see cref="IHttpResponseStreamWriterFactory"/>.
    /// </summary>
    protected IHttpResponseStreamWriterFactory WriterFactory { get; }

    /// <summary>
    /// Executes the <see cref="XmlResult"/> and writes the response.
    /// </summary>
    /// <param name="context">The <see cref="ActionContext"/>.</param>
    /// <param name="result">The <see cref="XmlResult"/>.</param>
    /// <returns>A <see cref="Task"/> which will complete when writing has completed.</returns>
    public Task ExecuteAsync(ActionContext context, XmlResult result)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (result == null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        var response = context.HttpContext.Response;

        string? resolvedContentType = null;
        Encoding? resolvedContentTypeEncoding = null;
        ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                                                result.ContentType,
                                                response.ContentType,
                                                (DefaultContentType, DefaultEncoding),
                                                ResponseContentTypeHelper.GetEncoding,
                                                out resolvedContentType,
                                                out resolvedContentTypeEncoding);

        response.ContentType = resolvedContentType;

        if (result.StatusCode != null)
        {
            response.StatusCode = result.StatusCode.Value;
        }

        var serializerSettings = result.XmlSerializerSettings ?? FormattingUtilities.GetDefaultXmlWriterSettings();



        var outputFormatterWriterContext = new OutputFormatterWriteContext(
                                                    context.HttpContext,
                                                    WriterFactory.CreateWriter,
                                                    result?.Value?.GetType(), result?.Value);

        outputFormatterWriterContext.ContentType = new StringSegment(resolvedContentType);

        //  Logger formatter and value of object

        Logger.FormatterSelected(OutputFormatter, outputFormatterWriterContext);
        Logger.XmlResultExecuting(result?.Value);

        return OutputFormatter.WriteAsync(outputFormatterWriterContext);
    }


}

