﻿using Lib.BusinessObjects.Translations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Weblib.Helpers
{
    public class CultureHelper
    {
        // Properties  
        public static Language Language
        {
            get
            {
                var llang = new Language() { Culture = Thread.CurrentThread.CurrentUICulture.Name };
                return (Language)llang.Populate(llang).Values.FirstOrDefault();
            }
            set
            {

                if (value != null && value.Id>0 && !string.IsNullOrEmpty(value.Culture))
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(value.Culture);
                }
                else
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                }

                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
        }
    }
}
