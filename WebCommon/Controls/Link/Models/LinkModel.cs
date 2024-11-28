namespace Controls.Link.Models
{
    using Lib.Tools.Controls;

    using Weblib.Models.Common;

    public class LinkModel : IDataModel
    {
        public Lib.Models.LinkModel Link { get; set; }
        public bool ReadOnly { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string CssView { get; set; }
        public string CssEdit { get; set; }
        public string Label { get; set; }
        public Lib.AdvancedProperties.DisplayMode Mode { get; set; }
        public bool hasValue()
        {
            return false;
        }
    }
}
