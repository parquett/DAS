﻿namespace Controls.Select.Models
{
    using Lib.Tools.BO;
    using Lib.Tools.Controls;
    using System.Collections.Generic;
    using Weblib.Models.Common;

    public class MultyCheckModel : IDataModel
    {
        public Dictionary<long, ItemBase> Values { get; set; }
        public MultyCheckWidgetModel MultyCheck { get; set; }
        public bool ReadOnly { get; set; }
        public string CssView { get; set; }
        public string CssEdit { get; set; }
        public string Label { get; set; }
        public Lib.AdvancedProperties.DisplayMode Mode { get; set; }
        public bool hasValue()
        {
            if (Values == null)
                return false;

            if (Values.Count == 0)
                return false;

            return true;
        }
    }
}
