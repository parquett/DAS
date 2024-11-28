// ------------------------------------------------------------------------------------------------------public --------------
// <copyright file="Authorization.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The Authorization.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMLib.Security
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Tools.Utils;
    using System.Collections.Generic;
    using SecurityCRMLib.BusinessObjects;
    using Lib.Tools.BO;

    /// <summary>
    /// The Authorization.
    /// </summary>
    public class Authorization
    {
        public static bool hasPageAccess(/*Dictionary<long, MenuGroup> MenuItems, */Lib.BusinessObjects.User usr, ItemBase item)
        {
            if (usr.HasAtLeastOnePermission((long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin))
                return true;

            return false;
            //return MenuItems.Values.Any(g => g.MenuItems != null 
            //                            && g.MenuItems.Values.Any(
            //                                    mi => ((MenuItem)mi).Page != null 
            //                                    && ((MenuItem)mi).Page.PageObject != null 
            //                                    && ((MenuItem)mi).Page.PageObject.Type == item.GetPermissionsType() 
            //                                    && usr.HasAtLeastOnePermission(((MenuItem)mi).Page.Permission)
            //                                    )
            //                            );
        }
    }
}