// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions
{
    [TestClass]
    public class FromXmlBodyAttributeTest
    {
        [TestMethod]
        public void MultiTest()
        {
            InlineData(XmlSerializerType.XmlSeriralizer, false, typeof(XmlBodyModelBinder));
            InlineData(XmlSerializerType.XmlSeriralizer, true, typeof(XmlBodyModelBinderOnly));
            InlineData(XmlSerializerType.DataContractSerializer, false, typeof(DcXmlBodyModelBinder));
            InlineData(XmlSerializerType.DataContractSerializer, true, typeof(DcXmlBodyModelBinderOnly));
        }

        void InlineData(XmlSerializerType xmlSerializerType, bool useXmlBinderOnly, Type expectedType)
        {
            // Act
            var att = new FromXmlBodyAttribute()
            {
                XmlSerializerType = xmlSerializerType,
                UseXmlBinderOnly = useXmlBinderOnly
            };
            //Assert

            Assert.AreEqual(expectedType, att.BinderType);
            Assert.AreEqual(BindingSource.Body, att.BindingSource);

        }
    }
}