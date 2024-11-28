using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRMLib.Extensions
{
    public static class AccountManagementExtensions
    {

        public static string GetProperty(this Principal principal, string property)
        {
            if (principal.GetUnderlyingObject() is DirectoryEntry directoryEntry 
                && directoryEntry.Properties.Contains(property))
                return directoryEntry.Properties[property].Value.ToString();
            else
                return string.Empty;
        }

        public static string GetCompany(this Principal principal)
        {
            return principal.GetProperty("company");
        }

        public static string GetDepartment(this Principal principal)
        {
            return principal.GetProperty("department");
        }

    }
}
