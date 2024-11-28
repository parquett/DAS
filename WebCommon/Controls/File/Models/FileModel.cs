namespace Controls.File.Models
{
    using Lib.BusinessObjects;
    using Lib.Tools.Controls;
    using Weblib.Models.Common;

    public class FileModel : IDataModel
    {
        public Document Value { get; set; }
        public string BOName { get; set; }
        public string PropertyName { get; set; }
        public bool ReadOnly { get; set; }
        public string CssView { get; set; }
        public string CssEdit { get; set; }
        public string Label { get; set; }
        public Lib.AdvancedProperties.DisplayMode Mode { get; set; }
        public string UniqueId { get; set; }
        public bool hasValue()
        {
            if (Value == null)
                return false;

            return true;
        }
    }
}
