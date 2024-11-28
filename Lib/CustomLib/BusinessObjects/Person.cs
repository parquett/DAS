using SecurityCRMLib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Users.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The user.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMLib.BusinessObjects
{
    using System;
    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Tools.Utils;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Data;
    using System.Collections.Generic;
    using Lib.Tools.BO;
    using Lib.Tools.AdminArea;
    using Lib.Tools.Security;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.Windows.Input;
    using Lib.Helpers;
    using lib;
    using ApiContracts.Enums;

    /// <summary>
    /// The user.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.UserManagement
      , ModulesAccess = (long)(Modulesenum.SMI)
      , DisplayName = "Persons"
      , SingleName = "Person"
      , EditAccess = (long)(SecurityCRM.ApiContracts.Enums.Permissions.CPAccess | SecurityCRM.ApiContracts.Enums.Permissions.SMIAccess)
      , CreateAccess = (long)(SecurityCRM.ApiContracts.Enums.Permissions.CPAccess | SecurityCRM.ApiContracts.Enums.Permissions.SMIAccess)
      , DeleteAccess = (long)(SecurityCRM.ApiContracts.Enums.Permissions.CPAccess | SecurityCRM.ApiContracts.Enums.Permissions.SMIAccess)
      , ReadAccess = (long)(SecurityCRM.ApiContracts.Enums.Permissions.CPAccess | SecurityCRM.ApiContracts.Enums.Permissions.SMIAccess)
      , LogRevisions = true
      , RevisionsAccess = (long)(SecurityCRM.ApiContracts.Enums.Permissions.CPAccess | SecurityCRM.ApiContracts.Enums.Permissions.SMIAccess)
      , Icon = "male")]
    public class Person : Lib.BusinessObjects.Person
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Person(long id)
            : base(id)
        {
        }
        #endregion

        public static Person Current()
        {
            SessionHelper<Person>.Pull(SessionItems.Person, out var CurrentPerson);
            return CurrentPerson;
        }

        public override string GetName()
        {
            return LastName+" "+FirstName;
        }

        public override string GetCaption()
        {
            return "LastName";
        }

        public override string GetAdditionalSelectQuery(AdvancedProperty property)
        {
            return ",[" + property.PropertyName + "].FirstName" + " AS " + property.PropertyName + "FirstName";
        }
        public Dictionary<long, ItemBase> PopulateAutocomplete(string param, string search)
        {
            const string strSql = "Person_Populate_Autocomplete";

            var conn = DataBase.ConnectionFromContext();

            var selectCommand = new SqlCommand(strSql, conn) { CommandType = CommandType.StoredProcedure };
            selectCommand.Parameters.Add(new SqlParameter("@search", SqlDbType.NVarChar, 50) { Value = search });

            var persons = new Dictionary<long, ItemBase>();

            using (var rdr = selectCommand.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    var person = (new Person()).FromDataRow(rdr);
                    persons.Add(person.Id, person);
                }
                rdr.Close();
            }
            return persons;
        }

        public override Dictionary<long, ItemBase> PopulateAutocomplete(string Param, string search, string AdvancedFilter = "")
        {

            if (!string.IsNullOrEmpty(Param))
            {
                if (Param.IndexOf("Section|") != -1)
                {
                    const string strSql = "Person_Populate_Autocomplete";

                    var conn = DataBase.ConnectionFromContext();

                    var selectCommand = new SqlCommand(strSql, conn) { CommandType = CommandType.StoredProcedure };
                    selectCommand.Parameters.Add(new SqlParameter("@search", SqlDbType.NVarChar, 50) { Value = search });
                    selectCommand.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.BigInt) { Value = Convert.ToUInt64(Param.Split('|')[1]) });
                    var persons = new Dictionary<long, ItemBase>();

                    using (var rdr = selectCommand.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        while (rdr.Read())
                        {
                            var person = (new Person()).FromDataRow(rdr);
                            persons.Add(person.Id, person);
                        }
                        rdr.Close();
                    }
                    return persons;
                }
            }

            return base.PopulateAutocomplete(Param, search, AdvancedFilter);
        }

        #region UPDATE Methods

        public static void UpdateEmail(Lib.BusinessObjects.User usr, System.String email)
        {
            var conn = DataBase.ConnectionFromContext();
            var cmd = new SqlCommand("Person_UpdateEmail", conn) { CommandType = CommandType.StoredProcedure };
            var param = new SqlParameter("@UserId", SqlDbType.BigInt) { Value = usr.Id };
            cmd.Parameters.Add(param);
            param = new SqlParameter("@Email", SqlDbType.NVarChar, 50) { Value = email };
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region POPULATE

        public static void AddPersonInfo(Lib.BusinessObjects.User user)
        {
            const string StrSql = "Person_LoadInfo";

            var conn = DataBase.ConnectionFromContext();

            var cmd = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };

            var param = new SqlParameter("@UserId", SqlDbType.BigInt) { Value = user.Id };
            cmd.Parameters.Add(param);

            var ds = new DataSet();

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            ds.Tables[0].TableName = "Person";
            
            foreach (DataRow dr in ds.Tables["Person"].Rows)
            {
                var person = (Person)(new Person()).FromDataRow(dr);

                var context = HttpContextHelper.Current;

                var str = JsonConvert.SerializeObject(person);
                context.Session.SetString(SessionItems.Person, str);
            }
        }

        #endregion

    }
}