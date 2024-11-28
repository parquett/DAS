// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Text.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Text type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System;

    /// <summary>
    /// The text.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Text : PropertyItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether money.
        /// </summary>
        public bool Money { get; set; }
    }
}