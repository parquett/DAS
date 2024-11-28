// ------------------------------public --------------------------------------------------------------------------------------
// <copyright file="PagingHelper.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the PagingHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Weblib.Helpers
{
    using System;

    /// <summary>
    /// The PagingHelper.
    /// </summary>
    public class PagingHelper
    {
        public static string BuildPaginng(string pObject, long Count, int CountPerPage, int PageNum)
        {
            var pagingstr = "";

            if (Count / CountPerPage <= 6)
            {
                for (int p = 1; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }
            else if (PageNum <= 3 || PageNum >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
            {
                for (int p = 1; p <= 3; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                pagingstr += "<li>";
                pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(3)' class='pagination-page'>...</a>";
                pagingstr += "</li>";

                for (int p = Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }
            else
            {
                for (int p = 1; p <= 3; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                var pstart = PageNum - 1;
                var pend = PageNum + 1;

                if (pstart <= 3)
                {
                    pstart++;
                    pend++;
                }
                else if (pend >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
                {
                    pstart--;
                    pend--;
                }

                if (pstart <= 3)
                {
                    pstart++;
                }

                if (pend >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
                {
                    pend--;
                }

                if (pstart != 4)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(3)' class='pagination-page'>...</a>";
                    pagingstr += "</li>";
                }

                for (int p = pstart; p <= pend; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                if (pend != Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 3)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(" + (Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2).ToString() + ")' ' class='pagination-page'>...</a>";
                    pagingstr += "</li>";
                }

                for (int p = Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_"+ pObject + "_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }

            return pagingstr;
        }

    }
}
