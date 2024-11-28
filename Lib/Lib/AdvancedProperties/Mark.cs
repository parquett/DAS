// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mark.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Mark type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System;

    /// <summary>
    /// The mark.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Mark : PropertyItem
    {
        /// <summary>
        /// Gets or sets the mark by class.
        /// </summary>
        public string MarkByClass { get; set; }

        /// <summary>
        /// Gets or sets the mark condition.
        /// </summary>
        public object MarkCondition { get; set; }
    }
}