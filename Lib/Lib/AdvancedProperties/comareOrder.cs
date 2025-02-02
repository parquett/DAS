// --------------------------------------------------------------------------------------------------------------------
// <copyright file="comareOrder.cs" company="SecurityCRM">
//   Copyright �  2020
// </copyright>
// <summary>
//   Defines the comareOrder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.AdvancedProperties
{
    using System.Collections;

    /// <summary>
    /// The compare order.
    /// </summary>
    public class CompareOrder : IComparer
    {
        /// <summary>
        /// The compare.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int Compare(object x, object y)
        {
            return ((AdvancedProperty)x).Common.Order.CompareTo(((AdvancedProperty)y).Common.Order);
        }
    }
}