using lib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextItemsHolder.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the ContextItemsHolder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Tools.Utils
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// The context items holder.
    /// </summary>
    public class ContextItemsHolder
    {
        /// <summary>
        /// The connection context id.
        /// </summary>
        public const string ConnectionContextId = "SqlConn";

        public const string DropDownLookUp = "DropDownLookUp";

        /// <summary>
        /// The object from context.
        /// </summary>
        /// <param name="objectContextId">
        /// The object context id.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ObjectFromContext(string objectContextId)
        {
            return null == HttpContextHelper.Current ? null : HttpContextHelper.Current.Items[objectContextId];
        }

        /// <summary>
        /// The object to context.
        /// </summary>
        /// <param name="objectContextId">
        /// The object context id.
        /// </param>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ObjectToContext(string objectContextId, object obj)
        {
            if (null == HttpContextHelper.Current)
            {
                return false;
            }

            HttpContextHelper.Current.Items[objectContextId] = obj;
            return true;
        }
    }
}