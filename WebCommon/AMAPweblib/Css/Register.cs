﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Register.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Register type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMweblib.Css
{
    using System.Collections.Generic;

    using Weblib.Helpers;

    /// <summary>
    /// The register.
    /// </summary>
    public class Register
    {
        /// <summary>
        /// The register bundles.
        /// </summary>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static List<ResourceRegister> RegisterBundles()
        {
            var cssScripts = Weblib.Css.Register.RegisterBundles();

            cssScripts.AddRange(new List<ResourceRegister>
                                 {
                                     { new ResourceRegister() { File = "~/Content/FrontEnd/Controls.css", Key = "front_common" } },
                                     { new ResourceRegister() { File = "~/Content/FrontEnd/dashboard.css", Key = "front_common" } },
                                     { new ResourceRegister() { File = "~/Content/FrontEnd/Diagnostic.css", Key = "front_common" } },
                                     { new ResourceRegister() { File = "~/Content/FrontEnd/Procedure.css", Key = "front_common" } },
                                     { new ResourceRegister() { File = "~/Content/FrontEnd/Report.css", Key = "front_common" } },
                                 });

            return cssScripts;
        }
    }
}
