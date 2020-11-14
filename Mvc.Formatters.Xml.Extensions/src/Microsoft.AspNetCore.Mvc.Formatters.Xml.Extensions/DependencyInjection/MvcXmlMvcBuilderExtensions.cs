// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding Xml formatters to MVC.
    /// </summary>
    public static class MvcXmlMvcBuilderExtensions
    {
        /// <summary>
        /// Adds the XmlSerializer and DataContractSerializer formatters to MVC.
        /// Adds the XmlResult and FromXmlBody Extension to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddXmlFormaterExtensions(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.AddXmlDataContractSerializerFormatters();
            builder.AddXmlSerializerFormatters();
            AddXmlFormaterExtensionsServices(builder.Services);
            return builder;
        }

        // Internal for testing.
        internal static void AddXmlFormaterExtensionsServices(IServiceCollection services)
        {
         
            services.TryAddSingleton<XmlDcResultExecutor>();
            services.TryAddTransient<DcXmlBodyModelBinder>();
            services.TryAddTransient<DcXmlBodyModelBinderOnly>();

            services.TryAddSingleton<XmlResultExecutor>();
            services.TryAddTransient<XmlBodyModelBinder>();
            services.TryAddTransient<XmlBodyModelBinderOnly>();
        }
    }
}
