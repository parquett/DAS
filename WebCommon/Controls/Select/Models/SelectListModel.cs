﻿namespace Controls.Select.Models
{
    using Lib.Tools.BO;
    using Lib.Tools.Controls;
    using Weblib.Models.Common;

    public class SelectListModel : SelectModel
    {
        public TextboxModel SelectList { get; set; }
        public new bool hasValue()
        {
            if (MultyCheckModel != null)
                return MultyCheckModel.hasValue();

            if (Value == null)
                return false;

            if (Value.Id == 0)
                return false;

            return true;
        }
    }
}
