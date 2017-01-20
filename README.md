### XmlResult and FromXmlBody MVC XML formatter extensions.

### Nuget Package: 
https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions 

###  MVC XML formatter's extensions allow:

1. In ASP.NET MVC CORE Web REST application to have deal with flat XML and DataContract XML.
2. Remove ObjectResult limitation to use only one type of MVC XML serializer per WEB Application. 
3. Satisfy all possible XML JAVA WEB REST API and XML .NET WEB REST API communication scenarios.
4. Improve the control logics of the type MVC XML used formatters.

### XmlResult
An Action result which formats the given object as XML.

1. The XmlResult is the similar feature to JsonResult in project "Microsoft.AspNetCore.Mvc.Formatters.Json".
2. It allows to use more then one type of the MVC XML formatter per Web REST Application.
3. It allows to return XML formatted response with the HTTP Response Body. 
4. The property of the XmlResult defines which one of the MVC XML formatters to use either XmlSerializer or DataContractSerializer.

### FromXmlBody 
Specifies that a parameter or property should be bound using the request body XML.

1. The FromBodyXmlAttribute is the similar attribute to FromBodyAttribute in project "Microsoft.AspNetCore.Mvc".
2. FromBodyXmlAttribute forces try to use XML serializer for the HTTP Request Body with using ether "DataContractSerializer" or "XmlSerializer".
3. It allows to use more then one type of the MVC XML formatter per Web REST Application.

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
```
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
```
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
```
