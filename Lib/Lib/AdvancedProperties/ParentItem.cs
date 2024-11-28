// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parent.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Parent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System;

    /// <summary>
    /// The parent.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Parent : PropertyItem
    {
        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        public string Prefix { get; set; }
    }
}