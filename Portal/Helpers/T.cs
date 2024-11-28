using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SecurityCRM.Helpers
{
    public class T
    {
        public static string Str(string alias, string ResourceFile, string defaultvalue="")
        {
            return Lib.Tools.Utils.Translate.GetTranslatedValue(alias, ResourceFile,!String.IsNullOrEmpty(defaultvalue)? defaultvalue: alias);
        }
    }
}