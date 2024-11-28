//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="Message.cs" company="SecurityCRM">
////   Copyright ©  2020
//// </copyright>
//// <summary>
////   The Message class.
//// </summary>
//// --------------------------------------------------------------------------------------------------------------------

//namespace SecurityCRMLib.BusinessObjects
//{
//    using System;
//    using Lib.AdvancedProperties;
//    using Lib.BusinessObjects;
//    using Lib.Tools.BO;
//    using System.Collections.Generic;
//    using System.Data;
//    using System.Linq;
//    using Lib.Tools.Utils;
//    using System.Data.SqlClient;
//    using Lib.Tools.AdminArea;
//    using ApiContracts.Enums;

//    [Serializable]
//    [Bo(Group = AdminAreaGroupenum.Teams
//      , ModulesAccess = (long)(Modulesenum.SMI)
//      , DisplayName = "PersonTeam"
//      , SingleName = "PersonTeam")]
//    public class PersonTeam : ItemBase
//    {
//        #region Constructors

//        public PersonTeam()
//            : base(0)
//        {
//        }

//        public PersonTeam(long id)
//            : base(id)
//        {
//        }
//        #endregion

//        #region Properties

//        [Template(Mode = Template.ParentDropDown)]
//        public Team Team { get; set; }

//        [Template(Mode = Template.ParentDropDown)]
//        public Person Person { get; set; }
//        #endregion

//    }
//}