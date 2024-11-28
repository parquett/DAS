using SecurityCRM.ApiContracts.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO
{
    public class MonitoringOptionsDTO
    {
        public Legend? legend { get; set; }
        public Theme? theme { get; set; }
        public Chart? chart { get; set; }
        public DataLabels? dataLabels { get; set; }
        public Stroke? stroke { get; set; }
        public Tooltip? tooltip { get; set; }
        public Grid? grid { get; set; }
        public Xaxis? xaxis { get; set; }
        public Yaxis? yaxis { get; set; }

    }
    public class Yaxis
    {
        public bool show { get; set; }

    }
    public class Xaxis
    {
        public AxisBorder? axisBorder { get; set; }
        public AxisTicks? axisTicks { get; set; }
        public Labels? labels { get; set; }
        public string? type { get; set; }
        public string? range { get; set; }
        public List<string>? categories { get; set; }

        public int? tickAmount { get; set; }

    }
    public class Labels
    {
        public Style? style { get; set; }

    }
    public class AxisTicks
    {
        public bool show { get; set; }

    }
    public class AxisBorder
    {
        public bool show { get; set; }

    }
    public class Grid
    {
        public bool show { get; set; }

    }
    public class Tooltip
    {
        public Style? style { get; set; }
        public string? theme { get; set; }
        public X? x { get; set; }

    }
    public class X
    {
        public string? format { get; set; }

    }
    public class Style
    {
        public string? fontSize { get; set; }
        public string? fontFamily { get; set; }
        public string? fontWeight { get; set; }
        public string? backgroundColor { get; set; }
        public string? colors { get; set; }

    }
    public class Stroke
    {
        public string? curve { get; set; }

        public int? width { get; set; }

    }
    public class DataLabels
    {
        public bool enabled { get; set; }

    }
    public class Legend
    {
        public bool show { get; set; }

    }
    public class Theme
    {
        public string? mode { get; set; }

    }
    public class Chart
    {
        public string? type { get; set; }
        public Toolbar? toolbar { get; set; }

        public int? height  {get;set;}

    }
    public class Toolbar
    {
        public bool show { get; set; }

    }
}
