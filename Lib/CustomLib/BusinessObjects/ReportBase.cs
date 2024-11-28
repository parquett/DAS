using Lib.Tools.BO;

using Microsoft.AspNetCore.Mvc.ViewEngines;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecurityCRMLib.BusinessObjects
{
    public abstract class ReportBase : ModelBase
    {
        protected ReportBase(ICompositeViewEngine viewEngine)
        {
        }

        protected ReportBase()
        {
        }

        public virtual string getConditionalClass()
        {
            return "";
        }

        public virtual ItemBase getSearchItem(ItemBase Item)
        {
            return Item;
        }
    }
}
