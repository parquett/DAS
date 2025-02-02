﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Link.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Link type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System;

    /// <summary>
    /// The link.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Link : PropertyItem
    {
        /// <summary>
        /// Gets or sets the link type.
        /// </summary>
        public Type LinkType { get; set; }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        public string Namespace { get; set; }
    }
}