using Lib.AdvancedProperties;
using Lib.Tools.BO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecurityCRMLib.BusinessObjects
{
    public class PrintTest: PrintBase
    {
        public PrintTest()
            : base(0)
        {
        }

        public PrintTest(long Id)
            : base(Id)
        {
        }

        public override string LoadReport(ICompositeViewEngine _viewEngine, ControllerContext ControllerContext, ViewDataDictionary ViewData, ITempDataDictionary TempData, ExportType type = ExportType.None)
        {
            throw new NotImplementedException();
        }
    }
}
