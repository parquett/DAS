// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestResultType.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the RequestResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Helpers
{
    /// <summary>
    /// The request result type.
    /// </summary>
    public enum RequestResultType
    {
        /// <summary>
        /// The fail.
        /// </summary>
        Fail = 0,

        /// <summary>
        /// The success.
        /// </summary>
        Success = 1,

        /// <summary>
        /// The Reload.
        /// </summary>
        Reload = 2,

        /// <summary>
        /// The Alert.
        /// </summary>
        Alert = 3
    }
}
