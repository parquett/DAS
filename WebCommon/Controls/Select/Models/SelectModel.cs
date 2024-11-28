namespace Controls.Select.Models
{
    using Lib.Models;
    using Lib.Tools.BO;
    using Lib.Tools.Controls;
    using Weblib.Models.Common;

    public class SelectModel : IDataModel
    {
        public ItemBase Value { get; set; }
        public DropDownModel DropDown { get; set; }
        public MultyCheckModel MultyCheckModel { get; set; }
        public Lib.Models.LinkModel Link { get; set; }
        public bool ReadOnly { get; set; }
        public string CssView { get; set; }
        public string CssEdit { get; set; }
        public string Label { get; set; }
        public Lib.AdvancedProperties.DisplayMode Mode { get; set; }
        public bool hasValue()
        {
            if (MultyCheckModel!=null)
                return MultyCheckModel.hasValue();

            if (Value == null)
                return false;

            if (Value.Id == 0)
                return false;

            return true;
        }
    }
}
