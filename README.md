### XmlResult and FromXmlBody MVC XML formatter extensions".

### Nuget Package: 
https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions 

### Reasons for developing these features for the aspnet/MVC project:

1. When ASP.NET CORE is using as the base for development for Web REST API there is no controllable by code flexibility to have deal with flat XML and DataContract XML.
2. Without work around, Indeed, it is possible to use only one type of XML formatters per application.
3.  Currently used ObjectResult cannot provide the compulsory return XML or DataContract XML as an action result. 
4. The order of applying:  
		services.AddMvc().AddXmlDataContractSerializerFormatters().AddXmlSerializerFormatters();
	and   
		services.AddMvc().AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();
	affects on using the type of xml formatters for Content Type : "application/xml"

To improve the quality and flexibility  XML based Web REST API were developed the following features:

### XmlResult 
1. The XmlResult is the similar feature to JsonResult in project "Microsoft.AspNetCore.Mvc.Formatters.Json".
2. It allows to return Xml formatted response in the HTTP response body.
3. This feature improve the  consistence of the type XML used formatter.
4. XmlResult allows to return XML serialized object with using ether "DataContractSerializer" ether "XmlSerializer". It allows to satisfy all REST communication scenarios from JAVA  to .NET.

### "FromXmlBody" 

1. FromBodyXmlAttribute forces try to get  XML serialized object from the http request body with using ether "DataContractSerializer" or "XmlSerializer".

### Example of using in the application:

Startup.cs
```
public void ConfigureServices(IServiceCollection services)
{
    // Add framework services.

    // "AddXmlFormaterExtensions()" initialize the Asp .Net Core MVC to use of XmlResult and FromXmlBody:
    //  - It adds the XmlSerializer and DataContractSerializer formatters to MVC.
    //  - It adds the XmlResult and FromXmlBody Extension to MVC.
    services.AddMvc().AddXmlFormaterExtensions(); 
}

XmlExtController.cs(Example): 
 ```
/// <summary>
/// The Controller example of using of XmlResult and FromXmlBody.
/// It demonstrates how to define which of the XML formatters DataContractSerializer
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

Where the Models:

 ```
   [DataContract (Namespace ="http://puchase.Interface.org/Purchase.Order")]
    public class PurchaseOrder
    {
        public PurchaseOrder()
        {
            billTo = new Address() { street = "Bill to Address" };
            shipTo = new Address() { street = "Ship to  Address" };
        }
        [DataMember]
        public Address billTo;
        [DataMember]
        public Address shipTo;
    }

　
    [DataContract(Namespace = "http://puchase.Interface.org/Purchase.Order.Address")]
    public class Address
    {
        [DataMember]
        public string street;
    }
