using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.Enums
{
    /// <summary>
    /// The Permission Enumerator.
    /// </summary>
    [Flags]
    public enum Permissions : long
    {
        None = 0,
        CPAccess = 1,
        SuperAdmin = 2,
        SMIAccess = 4,
        API=8,
        VIncident = 16,
        VIP = 32,
        VAdmins = 64,
        CIncindent = 128,

    }
}
