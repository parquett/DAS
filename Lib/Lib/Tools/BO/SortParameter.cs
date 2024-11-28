// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SortParameter.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The translation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Tools.BO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Lib.BusinessObjects;
    //using Lib.Tools.Memcached;
    using Lib.Tools.Utils;

    /// <summary>
    /// The translation.
    /// </summary>
    [Serializable]
    public class SortParameter
    {
        public string Direction { get; set; }
        public string Field { get; set; }
    }
}