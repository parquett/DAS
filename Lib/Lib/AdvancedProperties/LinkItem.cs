// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkItem.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the LinkItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System;

    /// <summary>
    /// The link item.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LinkItem : Link
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkItem"/> class.
        /// </summary>
        public LinkItem()
        {
            this.ViewField = "Name";
            this.ValueField = "ID"; // It is only the value what will be posted, but it is checked by ID
        }

        /// <summary>
        /// Gets or sets the view field.
        /// </summary>
        public string ViewField { get; set; }

        /// <summary>
        /// Gets or sets the value field.
        /// </summary>
        public string ValueField { get; set; }
    }
}