﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Mvc.Formatters.Xml.Extensions;

/// <summary>
/// Defines the type selector of the serializer
/// </summary>
public enum XmlSerializerType
{
    XmlSerializer = 0,
    DataContractSerializer = 1
}