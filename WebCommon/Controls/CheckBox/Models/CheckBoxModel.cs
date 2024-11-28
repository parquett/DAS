namespace Controls.CheckBox.Models
{
    using Lib.Tools.Controls;

    using Weblib.Models.Common;

    public class CheckBoxModel : IDataModel
    {
        public bool Value { get; set; }
        public CheckboxModel Checkbox { get; set; }
        public bool ReadOnly { get; set; }
        public string CssView { get; set; }
        public string CssEdit { get; set; }
        public string Label { get; set; }
        public Lib.AdvancedProperties.DisplayMode Mode { get; set; }
        public bool hasValue()
        {
            return true;
        }
    }
}
