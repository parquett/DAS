﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Tools.AdminArea
{
    public class AdminAreaGroup
    {
        public AdminAreaGroup()
        {
            Icon = "folder";
        }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Descripription { get; set; }
    }
}
