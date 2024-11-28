// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Service.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Calendar type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System;

    /// <summary>
    /// The service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Service : PropertyItem
    {
        /// <summary>
        /// Gets or sets the language abbreviation.
        /// </summary>
        public string LanguageAbbr { get; set; }
    }
}