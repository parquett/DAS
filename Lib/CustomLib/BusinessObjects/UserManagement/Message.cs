// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Message.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The Message class.
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
      , DisplayName = "Message"
      , SingleName = "Message")]
    public class Message : ItemBase
    {
        #region Constructors

        public Message()
            : base(0)
        {
        }

        public Message(long id)
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

        [Common(Order = 1), Template(Mode = Template.Description)]
        public string Text { get; set; }

        [Common(Order = 2), Template(Mode = Template.DateTime)]
        public DateTime Date { get; set; }

        [Common(Order = 3), Template(Mode = Template.ParentDropDown)]
        public User User { get; set; }

        #endregion

        #region Populate Methods

        /// <summary>
        /// The from data table.
        /// </summary>
        /// <param name="dt">
        /// The data table.
        /// </param>
        /// <param name="ds">
        /// The data set.
        /// </param>
        /// <param name="usr">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<long, ItemBase> FromDataTable(DataRow[] dt, DataSet ds, Lib.BusinessObjects.User usr)
        {
            var Messages = new Dictionary<long, ItemBase>();
            foreach (var dr in dt)
            {
                var obj = (new Message()).FromDataRow(dr);
                {
                    Messages.Add(obj.Id, obj);
                }
            }

            return Messages;
        }

        #endregion
    }
}