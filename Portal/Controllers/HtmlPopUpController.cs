using Lib.Tools.BO;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Weblib.Helpers;
using Lib.BusinessObjects;
using SecurityCRMLib.BusinessObjects;
using System.Globalization;

using Lib.AdvancedProperties;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SecurityCRM.Controllers
{
    public class HtmlPopUpController : SecurityCRMweblib.Controllers.FrontEndController
    {
        public HtmlPopUpController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }
    }
}
