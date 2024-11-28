// --------------------------------------------------------------------------------------------------------------------
// <copyright file="URLHelper.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Strings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Tools.Utils
{
    /// <summary>
    /// The strings.
    /// </summary>
    public static class URLHelper
    {
        public static string GetUrl(string relativeURL)
        {
            return GetUrl(relativeURL,"");
        }
        public static string GetUrl(string relativeURL, string WebSiteRootURL)
        {
            return (!string.IsNullOrEmpty(WebSiteRootURL)? WebSiteRootURL : Config.GetConfigValue("WebSiteRootURL")) + relativeURL;
        }
    }
}