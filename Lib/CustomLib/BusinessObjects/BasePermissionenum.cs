namespace Lib.BusinessObjects
{
    /// <summary>
    /// The Permission Enumerator.
    /// </summary>
    public enum BasePermissionenum : long
    {
        None = 0,
        CPAccess = 1,
        SuperAdmin = 2,
        SMIAccess = 4,
        API = 8,
        VIncident = 16,
        VIP = 32,
        VAdmins = 64,
        CIncindent = 128,
    }
}