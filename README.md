### ASP.NET Core MVC Xml formatter extensions

**Version 10.x.x** : supports only NetCore 10.0 

**Version 9.x.x** : supports only NetCore 9.0 

**Version 8.x.x** : supports only NetCore 8.0 

**Version 7.x.x** : supports only NetCore 7.0 

**Version 6.x.x** : supports only NetCore 6.0 


### Nuget Package: 
https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions 

### ASP.NET Core MVC Xml formatter's extensions allow:

1. ASP.NET MVC Core Web Application controller actions to control the Xml serialization type.
2. Avoid the ObjectResult limitation to use only one type of MVC Xml serializer per ASP.NET MVC Core Web Application. 
3. Satisfy all possible Xml JAVA REST Web API and Xml .NET REST Web API communication scenarios.

### XmlResult
An Action result which formats the given object as Xml.

1. The XmlResult is the similar feature to JsonResult in the project "Microsoft.AspNetCore.Mvc.Formatters.Json".
2. The property "XmlSerializerType" of the XmlResult defines which one of the MVC Xml formatters to use either XmlSerializer or DataContractSerializer.
3. It allows to return Xml formatted response with using the HTTP Response Body. 

### "FromXmlBody" 
Specifies an action parameter or property that should be bound with using the HTTP request Xml body.

1. The FromBodyXmlAttribute is the similar attribute to FromBodyAttribute in the project "Microsoft.AspNetCore.Mvc".
2. The property "XmlSerializerType" of the FromBodyXmlAttribute defines which one of the MVC Xml formatters to use either XmlSerializer or DataContractSerializer.

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

    // or services.AddControllers().AddXmlFormaterExtensions().AddNewtonsoftJson();
}
```

XmlExtController.cs(Example): 

```
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

