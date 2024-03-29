// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Microsoft.AspNetCore.Mvc.Formatters.Xml.Internal;

/// <summary>
/// Executes a <see cref="XmlResult"/> to write to the DataContract xml response.
/// </summary>
public class XmlDcResultExecutor : IXmlResultExecutor
{
    /// <summary>
    /// Creates a new <see cref="XmlResultExecutor"/>.
    /// </summary>
    /// <param name="writerFactory">The <see cref="IHttpResponseStreamWriterFactory"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
    public XmlDcResultExecutor(
        IHttpResponseStreamWriterFactory writerFactory,
        ILoggerFactory loggerFactory)
    {
        if (writerFactory == null)
        {
            throw new ArgumentNullException(nameof(writerFactory));
        }

        if (loggerFactory == null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        WriterFactory = writerFactory;
        LoggerFactory = loggerFactory;
    }

    /// <summary>
    /// Gets the <see cref="ILogger"/>.
    /// </summary>
    protected ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// Gets the <see cref="IHttpResponseStreamWriterFactory"/>.
    /// </summary>
    protected IHttpResponseStreamWriterFactory WriterFactory { get; }

    public Task ExecuteAsync(ActionContext context, XmlResult result)
    {
        var serializerSettings = result.XmlSerializerSettings ?? FormattingUtilities.GetDefaultXmlWriterSettings();
        TextOutputFormatter formatter;
        // create the proper formatter
        formatter = new XmlDataContractSerializerOutputFormatter(serializerSettings);
        XmlResultExecutorBase xmlBase = new XmlResultExecutorBase(WriterFactory, LoggerFactory, formatter);
        return xmlBase.ExecuteAsync(context, result);
    }
}
