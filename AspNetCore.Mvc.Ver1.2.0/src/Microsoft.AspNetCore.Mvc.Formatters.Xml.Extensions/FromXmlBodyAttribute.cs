// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions
{
    /// <summary>
    /// Specifies an action parameter or property that should be bound with using the HTTP request Xml body.
    /// Requires the Xml DataContractSerializer formatters or/and the Xml Serializer formatters to be add to MVC.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FromXmlBodyAttribute : Attribute, IBinderTypeProviderMetadata
    {
        /// <inheritdoc />
        public BindingSource BindingSource => BindingSource.Body;

        /// Gets the proper type of the Xml binder provider
        /// <inheritdoc />
        ///<remarks> Requires the Xml DataContractSerializer formatters or/and the Xml Serializer formatters to be add to MVC.</remarks>
        public Type BinderType => UseXmlBinderOnly ?
                                    (XmlSerializerType == XmlSerializerType.DataContractSerializer ? typeof(DcXmlBodyModelBinderOnly) : typeof(XmlBodyModelBinderOnly)) :
                                    (XmlSerializerType == XmlSerializerType.DataContractSerializer ? typeof(DcXmlBodyModelBinder) : typeof(XmlBodyModelBinder));

        /// <summary>
        /// Gets or sets the flag that selects a Data Contract Xml input formatter.
        /// </summary>
        public XmlSerializerType XmlSerializerType { get; set; }

        /// <summary>
        /// Gets or sets the flag that limits an input formatter to  Xml  or Data Contract Xml <see cref="XmlSerializerType"/>.
        /// </summary>
        public bool UseXmlBinderOnly { get; set; }

    }
}
