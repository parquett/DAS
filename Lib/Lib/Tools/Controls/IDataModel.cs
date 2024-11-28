using Lib.AdvancedProperties;
namespace Lib.Tools.Controls
{
    public interface IDataModel
    {
        bool ReadOnly {get; set;}

        string CssView { get; set; }
        string CssEdit { get; set; }
        string Label { get; set; }

        DisplayMode Mode { get; set; }

        bool hasValue();
    }
}
