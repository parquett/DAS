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
    public abstract class PrintBase : ModelBase
    {
        public PrintBase()
            : base(0)
        {
        }

        public PrintBase(long Id)
            : base(Id)
        {
        }

        public PrintBase(Dictionary<string, string> Filters, ICompositeViewEngine _viewEngine, ControllerContext ControllerContext, ViewDataDictionary ViewData, TempDataDictionary TempData)
            : base(0)
        {
            this.Filters = Filters;
            LoadReport(_viewEngine, ControllerContext, ViewData, TempData);
        }

        [Db(_Editable = false, _Ignore = true, _Populate = false)]
        public Dictionary<string, string> Filters { get; set; }

        [Db(_Editable = false, _Ignore = true, _Populate = false)]
        public string PrintTemplate { get; set; }

        [Db(_Editable = false, _Ignore = true, _Populate = false)]
        public string FileDownloadName { get; set; }

        [Db(_Editable = false, _Ignore = true, _Populate = false)]
        public string StylesFile { get; set; }

        public abstract string LoadReport(ICompositeViewEngine _viewEngine, ControllerContext ControllerContext, ViewDataDictionary ViewData, ITempDataDictionary TempData, ExportType type= ExportType.None);
    }
    public enum ExportType : int
    {
        None =0,
        Word = 1
    }
}
