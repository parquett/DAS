using lib;
using Lib.Tools.BO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Lib.Helpers
{
    public static class SessionHelper<T>
    {
        public static bool Pull(string pKey, out T pObject)
        {
            if (HttpContextHelper.Current!=null && HttpContextHelper.Current.Session.TryGetValue(pKey, out var rVal) && rVal != null)
            {
                pObject = JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(rVal));
                return true;
            }

            pObject = default(T);
            return false;
        }

        public static bool Push(string pKey, T pObject)
        {
            if(HttpContextHelper.Current == null)
                return false;

            try
            {
                var str = JsonConvert.SerializeObject(pObject
                    , Formatting.Indented);

                HttpContextHelper.Current.Session.SetString(pKey, str);
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
