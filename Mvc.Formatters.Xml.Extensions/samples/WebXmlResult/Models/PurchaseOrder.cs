// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace WebXmlResultVS2015.Controllers.Models
{

    [DataContract(Namespace = "http://puchase.Interface.org/Purchase.Order")]
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

}
