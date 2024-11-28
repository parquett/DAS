// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessObject.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the BusinessObject.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Lib.AdvancedProperties;

    public class BusinessObject
    {
        public Type Type { get; set; }
        public BoAttribute Properties { get; set; }
    }

 }
