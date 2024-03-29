// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

/// <summary>
/// An <see cref="IModelBinder"/> which binds models from the request body using an <see cref="XmlDataContractSerializerInputFormatter"/> as the first entry in the list of formatters
/// when a model has the binding source <see cref="BindingSource.Body"/>.
/// </summary>
public class DcXmlBodyModelBinder : IModelBinder
{
    BodyModelBinder BodyModelBinder { get; }

    /// <summary>
    /// Creates a new <see cref="XmlBodyModelBinder"/>.
    /// </summary>
    /// <param name="options">The configuration for the MVC framework.</param>
    /// <param name="readerFactory">
    /// The <see cref="IHttpRequestStreamReaderFactory"/>, used to create <see cref="System.IO.TextReader"/>
    /// instances for reading the request body.
    /// </param>
    public DcXmlBodyModelBinder(IOptions<MvcOptions> options, IHttpRequestStreamReaderFactory readerFactory)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (readerFactory == null)
        {
            throw new ArgumentNullException(nameof(readerFactory));
        }

        IList<IInputFormatter> formatters = options.Value.InputFormatters;
        var list = new List<IInputFormatter>() { new XmlDataContractSerializerInputFormatter(options.Value) };
        list.AddRange(formatters);
        BodyModelBinder = new BodyModelBinder(list, readerFactory);
    }

    /// <inheritdoc />
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        return BodyModelBinder.BindModelAsync(bindingContext);
    }
}
