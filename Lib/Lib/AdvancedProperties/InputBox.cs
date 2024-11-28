// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDown.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the InputBox type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System;

    /// <summary>
    /// The input.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InputBox : PropertyItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookUp"/> class.
        /// </summary>
        public InputBox()
        {
        }

        /// <summary>
        /// Gets or sets the OnChange value.
        /// </summary>
        public string OnChange { get; set; }
    }
}