// --------------------------------------------------------------------------------------------------------------------
// <copyright file="News.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The News class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMLib.BusinessObjects
{
    using System;
    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Tools.BO;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Lib.Tools.Utils;
    using System.Data.SqlClient;
    using Lib.Tools.AdminArea;
    using ApiContracts.Enums;

    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Settings
      , DisplayName = "News"
      , SingleName = "_News"
      , DoCancel = true  )]
    public class News : ItemBase
    {
        #region Constructors

        public News()
            : base(0)
        {
        }

        public News(long id)
            : base(id)
        {
        }
        #endregion

        public override string GetCaption()
        {
            return "Title";
        }

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Title { get; set; }

        [Common(Order = 0), Template(Mode = Template.Date)]
        public DateTime Date { get; set; }
        
        [Common(Order = 0), Template(Mode = Template.Html)]
        public string Text { get; set; }

        [Common(Order = 0), Template(Mode = Template.CheckBox)]
        public bool Active { get; set; }

        #endregion
    }
}