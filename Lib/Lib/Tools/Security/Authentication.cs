using lib;
// ------------------------------------------------------------------------------------------------------public --------------
// <copyright file="Authentication.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The authentication.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Tools.Security
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;
    using System.Security.Principal;

    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Helpers;
    using Lib.Tools.Utils;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Newtonsoft.Json;
    using ApiContracts.Enums;
    using SecurityCRM.Helpers;
    using SecurityCRM.ApiContracts.DTO.User;
    using System.Threading.Tasks;

    /// <summary>
    /// The authentication.
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// The do authorization.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="module">
        /// The administrator area login.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        //public static bool DoAuthorizationAPI(ref User user, SqlConnection conn)
        //{
        //    if (conn != null && conn.State == ConnectionState.Open)
        //    {
        //        const string StrSql = "User_DoLogin_API";

        //        var cmd = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };

        //        var param = new SqlParameter("@Login", SqlDbType.NVarChar, 50) { Value = GetUserName(user) };
        //        cmd.Parameters.Add(param);

        //        return Authorization(conn, cmd,null, Modulesenum.API, ref user);
        //    }
        //    else
        //    {
        //        throw new Exception("DataBase is not available");
        //    }
        //}
        //public static bool DoAuthorizationByTocken(ref User user, string pTocken, SqlConnection conn)
        //{
        //    if (conn != null && conn.State == ConnectionState.Open)
        //    {
        //        const string StrSql = "User_DoLogin_Tocken";

        //        var cmd = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };

        //        var param = new SqlParameter("@Tocken", SqlDbType.NVarChar, 50) { Value = pTocken };
        //        cmd.Parameters.Add(param);
        //        var bUserExists = false;
        //        using (var rdrUsers = cmd.ExecuteReader())
        //        {
        //            while (rdrUsers.Read())
        //            {
        //                user = (User)(new User()).FromDataRow(rdrUsers);
        //                bUserExists = true;
        //            }
        //        }

        //        return bUserExists;
        //    }
        //    else
        //    {
        //        throw new Exception("DataBase is not available");
        //    }
        //}

        /// <summary>
        /// The do authorization.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="module">
        /// The administrator area login.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static async Task<bool> DoAuthorization(User user, SqlConnection conn = null, Modulesenum module = Modulesenum.BaseModule)
        {
            if (conn == null)
            {
                conn = DataBase.ConnectionFromContext();
            }

            var context = HttpContextHelper.Current;

            if (conn != null && conn.State == ConnectionState.Open)
            {
                const string StrSql = "User_DoLogin";

                var cmd = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };

                var param = new SqlParameter("@Login", SqlDbType.NVarChar, 50) { Value = GetUserName(user) };
                cmd.Parameters.Add(param);

                return Authorization(conn, cmd, context, module, ref user);
            }
            else
            {
                throw new Exception("DataBase is not available");
            }
        }
        /// <summary>
        /// The check user.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        //public static User CheckUser(string page, Modulesenum module = Modulesenum.BaseModule)
        //{
        //    var context = HttpContextHelper.Current;

        //    if (context != null)
        //    {
        //        var user = GetCurrentUser(context, module);
        //        if (null == user)
        //        {
        //            if (string.IsNullOrEmpty(page))
        //            {
        //                context.Session.SetString("EnterPage", context.Request.GetDisplayUrl());
        //            }
        //            else
        //            {
        //                context.Session.SetString("EnterPage", page);
        //            }
        //        }

        //        return user;
        //    }

        //    return null;
        //}

        /// <summary>
        /// The get current user.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        public static User GetCurrentUser(HttpContext context = null, Modulesenum module = Modulesenum.BaseModule)
        {
            try
            {
                if (context != null && context.User != null)
                {
                   
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (SessionHelper<User>.Pull(SessionItems.User, out var user))
            {
                if (user != null)
                {
                    return new User()
                    {
                        Login = user.Login,
                        Role = new Role() { Id = user.Role.Id, Name = user.Role.Name, Permission = user.Role.Permission },
                        Person = new Person() { Id = user.Person.Id, FirstName = user.Person.FirstName, LastName = user.Person.LastName },
                        Id = user.Id
                    };
                }
                return null;
            }

            return null;
        }

        /// <summary>
        /// The get current user.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        public static void LogOff()
        {
            var context = HttpContextHelper.Current;

            if (context != null)
            {
                context.Session.Remove(SessionItems.User);
                context.Session.Remove(SessionItems.Token);
                context.Session.Remove(SessionItems.UserGuid);
                context.Session.Remove(SessionItems.Person);
            }
        }

        /// <summary>
        /// The get current user id.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static long GetCurrentUserId(HttpContext context = null, Modulesenum module = Modulesenum.BaseModule)
        {
            var user = GetCurrentUser(context, module);
            return user != null ? user.Id : 0;
        }

        /// <summary>
        /// The check current user.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        public static bool CheckUser(HttpContext context = null, Modulesenum module = Modulesenum.BaseModule)
        {
            return GetCurrentUser(context, module) != null;
        }

        /// <summary>
        /// The authorization.
        /// </summary>
        /// <param name="conn">
        /// The connection.
        /// </param>
        /// <param name="cmd">
        /// The command.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="admin">
        /// The administrator area login.
        /// </param>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool Authorization(SqlConnection conn, SqlCommand cmd, HttpContext context, Modulesenum module, ref User user)
        {
            try
            {
                using (var rdrUsers = cmd.ExecuteReader())
                {
                    while (rdrUsers.Read())
                    {
                        var str = rdrUsers["PasswordHash"].ToString().Trim();
                        if (user.PasswordHash == str)
                        {
                            user = (User)(new User()).FromDataRow(rdrUsers);

                            switch (module)
                            {

                                case Modulesenum.SMI:
                                    if (!user.HasPermissions((long)BasePermissionenum.SMIAccess))
                                    {
                                        return false;
                                    }
                                    break;

                                case Modulesenum.VIncident:
                                    if (!user.HasPermissions((long)BasePermissionenum.VIncident))
                                    {
                                        return false;
                                    }
                                    break;
                            }

                            UpdateLastLogin(conn, user);

                            InsertUserToSession(user, context);

                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static string GenerateTocken(SqlConnection conn, User user)
        {
            // set last_login time
            var cmd = new SqlCommand("User_Generate_Tocken", conn) { CommandType = CommandType.StoredProcedure };
            var param = new SqlParameter("@UserId", SqlDbType.NVarChar, 50) { Value = user.Id };
            cmd.Parameters.Add(param);
            return cmd.ExecuteScalar().ToString();
        }

        public static void UpdateLastLogin(SqlConnection conn, User user)
        {
            // set last_login time
            var cmd = new SqlCommand("User_Updatelast_login", conn) { CommandType = CommandType.StoredProcedure };
            var param = new SqlParameter("@UserId", SqlDbType.NVarChar, 50) { Value = user.Id };
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// The insert user to session.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public static void InsertUserToSession(User user,  HttpContext context = null)
        {
            //var maxTimeout = Convert.ToInt32(Config.GetConfigValue("MaxUserSessionTimeout"));
            //context.Session.tim = (user.Timeout>0 && user.Timeout < maxTimeout) ? user.Timeout : maxTimeout;
            SessionHelper<User>.Push(SessionItems.User, user);
        }

        /// <summary>
        /// The get user name.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetUserName(User user)
        {
            var result = user.Login;
            var pi = user.GetType().GetProperty("Login");
            if (
                pi.GetCustomAttributes(typeof(Encryption), false)
                  .OfType<Encryption>()
                  .Any(encryption => encryption.Encrypted))
            {
                result = Crypt.Encrypt(user.Login, Config.GetConfigValue("CryptKey"));
            }

            return !string.IsNullOrEmpty(result) ? result : "";
        }

        //    public static bool DoAuthorizationAd(User user, SqlConnection conn = null, HttpContext context = null
        //        , WindowsIdentity identity = null, UserPrincipal userInfo = null, long laboratoryId = 0, long permission = 0)
        //    {
        //        if (conn == null)
        //        {
        //            conn = DataBase.ConnectionFromContext();
        //        }

        //        if (context == null)
        //        {
        //            context = HttpContextHelper.Current;
        //        }

        //        if (conn != null && conn.State == ConnectionState.Open)
        //        {
        //            const string strSql = "User_DoLoginAD";

        //            var cmd = new SqlCommand(strSql, conn) { CommandType = CommandType.StoredProcedure };

        //            var firstName = userInfo?.GivenName ?? identity?.Name;
        //            var lastName = userInfo?.Surname ?? identity?.Name;

        //            if (firstName == lastName)
        //            {
        //                lastName = string.Empty;
        //            }

        //            cmd.Parameters.Add(new SqlParameter("@Login", SqlDbType.NVarChar)
        //            { Value = identity?.Name });
        //            cmd.Parameters.Add(new SqlParameter("@SID", SqlDbType.NVarChar)
        //            { Value = identity?.User?.Value });
        //            cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar)
        //            { Value = firstName });
        //            cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar)
        //            { Value = lastName });
        //            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar)
        //            { Value = userInfo?.EmailAddress ?? "" });
        //            if (laboratoryId > 0)
        //            {
        //                cmd.Parameters.Add(new SqlParameter("@LaboratoryId", SqlDbType.BigInt)
        //                { Value = laboratoryId });
        //            }
        //            cmd.Parameters.Add(new SqlParameter("@Permission", SqlDbType.BigInt)
        //            { Value = permission });
        //            user.SID = identity?.User?.Value;

        //            return Authorization(conn, cmd, context, Modulesenum.BaseModule,ref user);
        //        }

        //        throw new Exception("DataBase is not available");
        //    }
        //}
    }
}
