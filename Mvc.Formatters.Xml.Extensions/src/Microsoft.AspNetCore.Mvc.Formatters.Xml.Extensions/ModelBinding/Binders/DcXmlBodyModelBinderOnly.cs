// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

/// <summary>
/// An <see cref="IModelBinder"/> which binds models from the request body using an <see cref="XmlDataContractSerializerInputFormatter"/> only
/// when a model has the binding source <see cref="BindingSource.Body"/>.
/// </summary>
public class DcXmlBodyModelBinderOnly : BodyModelBinder
{
    /// <summary>
    /// Creates a new <see cref="DcXmlBodyModelBinderOnly"/>.
    /// </summary>
    /// <param name="options">The configuration for the MVC framework.</param>
    /// <param name="readerFactory">
    /// The <see cref="IHttpRequestStreamReaderFactory"/>, used to create <see cref="System.IO.TextReader"/>
    /// instances for reading the request body.
    /// </param>
    public DcXmlBodyModelBinderOnly(IOptions<MvcOptions> options, IHttpRequestStreamReaderFactory readerFactory) : base(new List<IInputFormatter>() { new XmlDataContractSerializerInputFormatter(options.Value) }, readerFactory)
    {
    }
}
