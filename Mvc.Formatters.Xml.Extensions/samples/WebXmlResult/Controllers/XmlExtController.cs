// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using WebXmlResultVS2015.Controllers.Models;
using Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions;

namespace WebXmlResultVS2015.Controllers
{

    /// <summary>
    /// The Controller example of using of XmlResult and FromXmlBody.
    /// It demonstrates how to define which of the Xml formatters DataContractSerializer
    /// or/and XmlSerializer to use for input and output in the Web Application controller actions.
    /// </summary>
    [Route("api/[controller]")]
    public class XmlExtController : Controller
    {
        // GET api/[controller]/xml
        [HttpGet("xml")]
        public ActionResult GetXmlObject()
        {
            object obj = new PurchaseOrder();
            return new XmlResult(obj);
        }

        // GET api/[controller]/dcxml
        [HttpGet("dcxml")]
        public ActionResult GetDcXmlObject()
        {
            object obj = new PurchaseOrder();
            return new XmlResult(obj) { XmlSerializerType = XmlSerializerType.DataContractSerializer };
        }

        // POST api/[controller]/xml
        [HttpPost("xml")]
        public void PostXml([FromXmlBody]PurchaseOrder value)
        {
            var x = value;
            x.billTo.street += " 123";
        }

        // POST api/[controller]/dcxml
        [HttpPost("dcxml")]
        public void PostDcXml([FromXmlBody(XmlSerializerType = XmlSerializerType.DataContractSerializer)]PurchaseOrder value)
        {
            var x = value;
            x.billTo.street += "No -10";
        }

    }
}
