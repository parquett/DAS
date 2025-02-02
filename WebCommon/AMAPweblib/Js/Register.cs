﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Register.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Register type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMweblib.Js
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
            var javaScripts = Weblib.Js.Register.RegisterBundles();

            javaScripts.AddRange(new List<ResourceRegister>
                                 {
                                     { new ResourceRegister() { File = "~/Scripts/JQuery/jquery.printArea.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/dashboard.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/investigation.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/request.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/document.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/consumable.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/animal.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/procedure.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/report.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/FrontEnd/Translation/static_translation.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/Common/translation.js", Key = "front_advanced" } },
                                     { new ResourceRegister() { File = "~/Scripts/Other_Plugins/d3.min.js", Key = "d3" } },
                                     { new ResourceRegister() { File = "~/Scripts/Other_Plugins/Donut3D.js", Key = "d3" } },
                                     { new ResourceRegister() { File = "~/Scripts/JQuery/jquery.mousewheel-3.0.6.pack.js", Key = "jquery-ui" } },
                                     { new ResourceRegister() { File = "~/Scripts/JQuery/jquery.fancybox.pack.js", Key = "jquery-ui" } },
                                     { new ResourceRegister() { File = "~/Scripts/JQuery/ui.datepicker-ro.js", Key = "jquery-ui" } },
                                     { new ResourceRegister() { File = "~/Scripts/JQuery/jquery.fancybox-buttons.js", Key = "jquery-ui" } }

                                 });

            return javaScripts;
        }
    }
}
