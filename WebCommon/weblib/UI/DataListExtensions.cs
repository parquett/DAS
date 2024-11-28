﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebLib.UI
{
    public static class DataListExtensions
    {
        public static HtmlString DataList<T>(this IHtmlHelper helper, IEnumerable<T> items, int columns,
            Func<T, HelperResult> template) 
            where T : class
        {
            if (items == null)
                return new HtmlString("");

            var sb = new StringBuilder();
            sb.Append("<table>");

            int cellIndex = 0;

            foreach (T item in items)
            {
                if (cellIndex == 0)
                    sb.Append("<tr>");

                sb.Append("<td");
                sb.Append(">");
                
                sb.Append(template(item).ToString());
                sb.Append("</td>");

                cellIndex++;

                if (cellIndex == columns)
                {
                    cellIndex = 0;
                    sb.Append("</tr>");
                }
            }

            if (cellIndex != 0)
            {
                for (; cellIndex < columns; cellIndex++)
                {
                    sb.Append("<td>&nbsp;</td>");
                }

                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return new HtmlString(sb.ToString());
        }
    }
}
