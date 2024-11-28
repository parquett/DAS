namespace ApiContracts.Enums
{
    /// <summary>
    /// The Module Enumerator.
    /// </summary>
    public enum Modulesenum : long
    {
        None = 0,

        /// <summary>
        /// Control Panel.
        /// </summary>
        ControlPanel = 1,
        
        /// <summary>
        /// System Management Interface.
        /// </summary>
        SMI = 2,

        /// <summary>
        /// Base Module.
        /// </summary>
        BaseModule = 4,

        /// <summary>
        /// API Module.
        /// </summary>
        API = 8,
        SuperAdmin = 16,
        VIncident = 32,
        VIP =   64,
        VAdmins = 128,
        CIncindent = 256,
    }
}