// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SortParameter.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The translation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Tools.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Lib.BusinessObjects;
    using Lib.Tools.Utils;

    /// <summary>
    /// The translation.
    /// </summary>
    [Serializable]
    public class DataTableOutput
    {
        public int sEcho { get; set; }
        public long iTotalRecords { get; set; }
        public long iTotalDisplayRecords { get; set; }
        public List<List<string>> aaData { get; set; }
    }
}