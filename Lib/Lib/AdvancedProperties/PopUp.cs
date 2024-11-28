// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopUp.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the PopUp type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System;

    /// <summary>
    /// The pop up.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PopUp : PropertyItem
    {
        /// <summary>
        /// Gets or sets the pop up caption.
        /// </summary>
        public string PopUpCaption { get; set; }
    }
}