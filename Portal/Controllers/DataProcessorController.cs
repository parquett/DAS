﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the AccountController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRM.Controllers
{
    using SecurityCRMweblib;
    using SecurityCRMweblib.Controllers;
    using lib;
    using Lib.AdvancedProperties;
    using Lib.Helpers;
    using Lib.Tools.Security;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Weblib.Controllers;
    using Lib.Tools.Utils;
    using System.ComponentModel;
    using Lib.Tools.BO;
    using Lib.Tools.Controls;
    using ItemBase = Lib.Tools.BO.ItemBase;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Lib.BusinessObjects;
    using Weblib.Helpers;
    using SecurityCRMLib.Security;
    using Controls.Input;

    /// <summary>
    /// The account controller.
    /// </summary>
    [Route("DataProcessor")]
    public class DataProcessorController : BaseController
    {
        public DataProcessorController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        [HttpPost]
        [Route("Load/{BO}/{Namespace}/{BOLink?}/{NamespaceLink?}/{Id?}")]
        public ActionResult Load(string BO, string Namespace, string BOLink = "ItemBase", string NamespaceLink = "Lib.BusinessObjects", string Id = "")
        {
            if (Authentication.CheckUser(this.HttpContext))
            {
                var item = Activator.CreateInstance(Type.GetType(Namespace + "." + BO + ", " + Namespace.Split('.')[0], true));
                BoAttribute boproperties = null;
                if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                {
                    boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                }

                if (!SessionHelper<string>.Pull(SessionItems.Module, out var Module))
                    Module = "ControlPanel";

                if (Module == "ControlPanel")
                {
                    HttpContextHelper.Current.Items["ControlPanel"] = true;
                }
                else
                {
                    HttpContextHelper.Current.Items["SystemManagement"] = true;
                }

                if (Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.ReadAccess))
                {
                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item.GetType());
                    var properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());

                    var is_search = false;
                    if (!string.IsNullOrEmpty(Id))
                    {

                        var LinkItem = Activator.CreateInstance(Type.GetType(NamespaceLink + "." + BOLink + ", " + NamespaceLink.Split('.')[0], true));
                        ((ItemBase)LinkItem).Id = Convert.ToInt64(Id);
                        foreach (AdvancedProperty property in properties)
                        {
                            if (
                                (property.Common.EditTemplate == EditTemplates.Parent
                                || property.Common.EditTemplate == EditTemplates.DropDownParent
                                || property.Common.EditTemplate == EditTemplates.SelectListParent)
                                && property.Type == LinkItem.GetType()
                                )
                            {
                                is_search = true;
                                property.PropertyDescriptor.SetValue(item, LinkItem);
                                break;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(Request.Form["isFileter"]))
                    {
                        is_search = true;

                        var search_properties = pss.GetSearchProperties(pdc);

                        foreach (AdvancedProperty property in search_properties)
                        {
                            property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", DisplayMode.Search));
                        }
                    }

                    var iDisplayStart = Convert.ToInt32(Request.Form["iDisplayStart"]);
                    var iDisplayLength = Convert.ToInt32(Request.Form["iDisplayLength"]);
                    var sSearch = Request.Form["sSearch"];
                    var sEcho = Convert.ToInt32(Request.Form["sEcho"]);
                    var SortParameters = new List<SortParameter>();

                    if (!string.IsNullOrEmpty(Request.Form["iSortCol_0"]))
                    {
                        var iSortingCols = Convert.ToInt32(Request.Form["iSortingCols"]);
                        for (var i = 0; i < iSortingCols; i++)
                        {
                            var iSortingCol = Convert.ToInt32(Request.Form["iSortCol_" + i.ToString()]);
                            if (Request.Form["bSortable_" + iSortingCol.ToString()] == "true")
                            {
                                if (iSortingCol == 1)
                                {
                                    SortParameters.Add(new SortParameter() { Direction = Request.Form["sSortDir_" + i.ToString()], Field = item.GetType().Name + "Id" });
                                }
                                else if (iSortingCol == properties.Count + 2)
                                {
                                    SortParameters.Add(new SortParameter() { Direction = Request.Form["sSortDir_" + i.ToString()], Field = "DateCreated" });
                                }
                                else if (iSortingCol == properties.Count + 3)
                                {
                                    SortParameters.Add(new SortParameter() { Direction = Request.Form["sSortDir_" + i.ToString()], Field = "CreatedBy" });
                                }
                                else
                                {
                                    var PropertyName = properties[iSortingCol - 2].PropertyName;
                                    if (properties[iSortingCol - 2].Type.BaseType == typeof(ItemBase)
                                        || (properties[iSortingCol - 2].Type.BaseType.BaseType != null && properties[iSortingCol - 2].Type.BaseType.BaseType == typeof(ItemBase))
                                        || (properties[iSortingCol - 2].Type.BaseType.BaseType != null && properties[iSortingCol - 2].Type.BaseType.BaseType != null && properties[iSortingCol - 2].Type.BaseType.BaseType.BaseType != null && properties[iSortingCol - 2].Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                        )
                                    {
                                        PropertyName += "Id";
                                        /*
                                        var sub_item = (ItemBase)Activator.CreateInstance(properties[iSortingCol - 2].Type);
                                        PropertyName = properties[iSortingCol - 2].Db.Prefix + properties[iSortingCol - 2].PropertyName + sub_item.GetCaption();*/
                                    }
                                    SortParameters.Add(new SortParameter() { Direction = Request.Form["sSortDir_" + i.ToString()], Field = PropertyName });
                                }
                            }
                        }
                    }

                    long totalItems = 0;
                    long totalDisplayItems = 0;
                    var DataItems = new Dictionary<long, ItemBase>();

                    if (boproperties.LoadFromDb)
                    {
                        DataItems = ((ItemBase)(item)).PopulateGridTools(null, (is_search ? (ItemBase)(item) : null), iDisplayStart, iDisplayLength, sSearch, SortParameters, false, null, out totalItems, out totalDisplayItems);
                    }
                    else
                    {
                        DataItems = ((ItemBase)(item)).Populate(SortParameters);

                        totalItems = DataItems.Count;
                        totalDisplayItems = DataItems.Count;
                    }

                    var Output = new DataTableOutput();

                    Output.sEcho = sEcho;
                    Output.iTotalRecords = totalItems;
                    Output.iTotalDisplayRecords = totalDisplayItems;
                    Output.aaData = new List<List<string>>();

                    foreach (var litem in DataItems.Values)
                    {
                        Output.aaData.Add(new List<string>());
                        Output.aaData[Output.aaData.Count - 1].Add(litem.Id.ToString());
                        Output.aaData[Output.aaData.Count - 1].Add(litem.Id.ToString());
                        foreach (AdvancedProperty property in properties)
                        {
                            var renderString = RenderPartialViewToString("~/Views/Controls/Generic/" + property.ControlView+".cshtml", property.GetDataProcessor().SetValue(property.PropertyDescriptor.GetValue(litem), property, litem)).Result;

                            Output.aaData[Output.aaData.Count - 1].Add(renderString);
                        }
                        Output.aaData[Output.aaData.Count - 1].Add(litem.DateCreated != DateTime.MinValue ? litem.DateCreated.ToString("dd/MM/yyyy HH:mm") : DateTimeZone.Now.ToString("dd/MM/yyyy HH:mm"));
                        Output.aaData[Output.aaData.Count - 1].Add(litem.CreatedBy != null ? "<a href='" + Lib.Tools.Utils.URLHelper.GetUrl(Module + "/Profile/" + litem.CreatedBy.Id.ToString()) + "'>" + litem.CreatedBy.Login + "</a>" : "");

                        if (item.GetType() == typeof(User) && !Authentication.GetCurrentUser().HasAtLeastOnePermission(((Lib.BusinessObjects.User)litem).Role.RoleAccessPermission))
                        {
                            Output.aaData[Output.aaData.Count - 1].Add("");
                            Output.aaData[Output.aaData.Count - 1].Add("");
                            Output.aaData[Output.aaData.Count - 1].Add("");
                        }
                        else
                        {
                            Output.aaData[Output.aaData.Count - 1].Add(litem.Id.ToString());
                            Output.aaData[Output.aaData.Count - 1].Add(litem.GetName());
                            Output.aaData[Output.aaData.Count - 1].Add(litem.GetName());
                        }
                    }
                    var jsonResult = Json(Output);
                    //jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }

            return this.Json(new RequestResult() { Message = "SeeionTimeOut", Result = RequestResultType.Reload });
        }

        [HttpPost]
        [Route("Insert")]
        public ActionResult Insert()
        {
            if (Authentication.CheckUser(this.HttpContext))
            {
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                var pss = new PropertySorter();
                var pdc = TypeDescriptor.GetProperties(item);
                var properties = pss.GetEditableProperties(pdc); 

                foreach (AdvancedProperty property in properties)
                {
                    if(property.PropertyName!= "DateCreated" 
                        && property.PropertyName != "SearchItems"
                        && property.PropertyName != "CreatedBy"
                        && property.PropertyName != "DeletedBy"
                        && property.PropertyName != "PasswordSalt"
                        && property.PropertyName != "PasswordHash")
                        property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property));
                    if(property.PropertyName == "PasswordHash")
                    {
                        var salt = PasswordHasher.GenerateSalt();
                        item.GetType().GetProperty("PasswordSalt").SetValue(item, salt, null);
                        item.GetType().GetProperty("PasswordHash").SetValue(item, DataProcessor.GetpasswordHash(Request.Form["Password"], salt), null);
                    }
                }
                item.Insert(item);

                return this.Json(new RequestResult() { Message = "OK", Result = RequestResultType.Reload });
            }

            return this.Json(new RequestResult() { Message = "SeeionTimeOut", Result = RequestResultType.Reload });
        }

        [HttpPost]
        [Route("Update")]
        public ActionResult Update()
        {

            if (Authentication.CheckUser(this.HttpContext))
            {
                if (!string.IsNullOrEmpty(Request.Form["id"]))
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                    item.Id = Convert.ToInt64(Request.Form["id"]);
                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);

                    AdvancedProperties properties = null;
                    if (!string.IsNullOrEmpty(Request.Form["simple"]))
                    {
                        properties = pss.GetEditableProperties(pdc, Authentication.GetCurrentUser(), DisplayMode.Simple);
                    }
                    else
                    {
                        properties = pss.GetEditableProperties(pdc, Authentication.GetCurrentUser(), DisplayMode.Advanced);
                    }

                    foreach (AdvancedProperty property in properties)
                    {
                        if (property.PropertyName != "DateCreated" && property.PropertyName != "SearchItems")
                            property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property));
                    }

                    item.Update(item, !string.IsNullOrEmpty(Request.Form["simple"]) ? DisplayMode.Simple : DisplayMode.Advanced);

                    return this.Json(new RequestResult() { Message = "OK", Result = RequestResultType.Success });
                }
                return this.Json(new RequestResult() { Message = "WrongParameters", Result = RequestResultType.Fail });
            }

            return this.Json(new RequestResult() { Message = "SeeionTimeOut", Result = RequestResultType.Reload });

        }

        [HttpPost]
        [Route("Clone")]
        public ActionResult Clone()
        {
            if (Authentication.CheckUser(this.HttpContext))
            {
                if (!SessionHelper<string>.Pull(SessionItems.Module, out var Module))
                    Module = "ControlPanel";

                if (Module == "ControlPanel")
                {
                    HttpContextHelper.Current.Items["ControlPanel"] = true;
                }
                else
                {
                    HttpContextHelper.Current.Items["SystemManagement"] = true;
                }

                if (!string.IsNullOrEmpty(Request.Form["id"]) && !string.IsNullOrEmpty(Request.Form["name"]))
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                    item.Id = Convert.ToInt64(Request.Form["id"]);
                    item = item.PopulateOne(item);
                    if (item != null)
                    {
                        item.Id = 0;
                        var comment = "Copied from \"" + item.GetName() + "\"";
                        item.SetName(Request.Form["name"]);

                        item.Insert(item, comment);

                        return this.Json(new RequestResult() { Message = Lib.Tools.Utils.URLHelper.GetUrl(Module.Substring(1, Module.Length - 1) + "/EditItem/" + item.GetType().Name + "/" + System.Net.WebUtility.UrlEncode(item.GetType().Namespace) + "/" + item.Id.ToString()), Result = RequestResultType.Success });
                    }
                }
                return this.Json(new RequestResult() { Message = "WrongParameters", Result = RequestResultType.Fail });
            }

            return this.Json(new RequestResult() { Message = "SeeionTimeOut", Result = RequestResultType.Reload });
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult Delete()
        {
            if (Authentication.CheckUser(this.HttpContext))
            {
                if (!string.IsNullOrEmpty(Request.Form["id"]))
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));
                    var sid = Request.Form["id"];
                    var itemsToDelete = new Dictionary<long, ItemBase>();
                    if (sid != "all")
                    {
                        item.Id = Convert.ToInt64(sid);
                        var pss = new PropertySorter();
                        var pdc = TypeDescriptor.GetProperties(item);

                        AdvancedProperties properties = null;
                        if (!string.IsNullOrEmpty(Request.Form["simple"]))
                        {
                            properties = pss.GetEditableProperties(pdc, Authentication.GetCurrentUser(), DisplayMode.Simple);
                        }
                        else
                        {
                            properties = pss.GetEditableProperties(pdc, Authentication.GetCurrentUser(), DisplayMode.Advanced);
                        }

                        foreach (AdvancedProperty property in properties)
                        {
                            if (property.PropertyName != "DateCreated" && property.PropertyName != "SearchItems")
                                property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property));
                        }

                        itemsToDelete.Add(item.Id, item);
                    }
                    else
                    {
                        itemsToDelete = item.Populate();
                    }
                    var Reason = "";
                    var result = item.Delete(itemsToDelete, out Reason);
                    if (result)
                    {
                        if (sid != "all")
                        {
                            return this.Json(new RequestResult() { Message = "OK", Result = RequestResultType.Success });
                        }
                        else
                        {
                            return this.Json(new RequestResult() { Message = "OK", Result = RequestResultType.Reload });
                        }
                    }
                    else
                    {
                        return this.Json(new RequestResult() { Message = Reason, Result = RequestResultType.Alert });
                    }
                }
                return this.Json(new RequestResult() { Message = "WrongParameters", Result = RequestResultType.Fail });
            }

            return this.Json(new RequestResult() { Message = "SeeionTimeOut", Result = RequestResultType.Reload });
        }

        [HttpPost]
        [Route("Print")]
        public ActionResult Print()
        {
            if (Authentication.CheckUser(this.HttpContext))
            {
                if (!string.IsNullOrEmpty(Request.Form["id"]))
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                    item.Id = Convert.ToInt64(Request.Form["id"]);
                    ViewData["BOType"] = item.GetType();
                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item.GetType());
                    var properties = pss.GetAdvancedProperties(pdc, Authentication.GetCurrentUser());
                    ViewData["Properties"] = properties;

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                    {
                        boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                    }
                    ViewData["BOProperties"] = boproperties;

                    item = ((ItemBase)(item)).PopulateOne((ItemBase)(item));

                    return this.View("~/Views/ControlPanel/PrintItem.cshtml", item);
                }
                else
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                    ViewData["BOType"] = item.GetType();
                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item.GetType());
                    var properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());
                    ViewData["Properties"] = properties;

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                    {
                        boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                    }
                    ViewData["BOProperties"] = boproperties;

                    var is_search = false;

                    if (!string.IsNullOrEmpty(Request.Form["isFileter"]))
                    {
                        is_search = true;

                        var search_properties = pss.GetSearchProperties(pdc);

                        foreach (AdvancedProperty property in search_properties)
                        {
                            property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property));
                        }
                    }

                    var DataItems = ((ItemBase)(item)).Populate((is_search ? (ItemBase)(item) : null));

                    return this.View("~/Views/ControlPanel/Print.cshtml", DataItems);
                }
            }

            return this.Json(new RequestResult() { Message = "SeeionTimeOut", Result = RequestResultType.Reload });
        }

        [HttpPost]
        [Route("ExportExcell")]
        public ActionResult ExportExcell()
        {
            if (Authentication.CheckUser(this.HttpContext))
            {
                if (!string.IsNullOrEmpty(Request.Form["id"]))
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                    item.Id = Convert.ToInt64(Request.Form["id"]);
                    ViewData["BOType"] = item.GetType();
                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item.GetType());
                    var properties = pss.GetAdvancedProperties(pdc, Authentication.GetCurrentUser());
                    ViewData["Properties"] = properties;

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                    {
                        boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                    }
                    ViewData["BOProperties"] = boproperties;

                    item = ((ItemBase)(item)).PopulateOne((ItemBase)(item));

                    var DataItems = new Dictionary<long, ItemBase>();

                    DataItems.Add(item.Id, item);

                    return File(ExcelExportHelper.ExportExcel(DataItems, properties, item.GetType().Name, true), ExcelExportHelper.ExcelContentType, item.GetType().Name + ".xlsx");
                }
                else
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                    ViewData["BOType"] = item.GetType();
                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item.GetType());
                    var properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());
                    ViewData["Properties"] = properties;

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                    {
                        boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                    }
                    ViewData["BOProperties"] = boproperties;

                    var is_search = false;

                    if (!string.IsNullOrEmpty(Request.Form["isFileter"]))
                    {
                        is_search = true;

                        var search_properties = pss.GetSearchProperties(pdc);

                        foreach (AdvancedProperty property in search_properties)
                        {
                            property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property));
                        }
                    }

                    var DataItems = ((ItemBase)(item)).Populate((is_search ? (ItemBase)(item) : null));

                    return File(ExcelExportHelper.ExportExcel(DataItems, properties, item.GetType().Name, true), ExcelExportHelper.ExcelContentType, item.GetType().Name + ".xlsx");
                }
            }

            return File("", "application/ms-excel");
        }

        [HttpPost]
        [Route("ExportCSV")]
        public ActionResult ExportCSV()
        {
            if (Authentication.CheckUser(this.HttpContext))
            {
                if (!string.IsNullOrEmpty(Request.Form["id"]))
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                    item.Id = Convert.ToInt64(Request.Form["id"]);
                    ViewData["BOType"] = item.GetType();
                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item.GetType());
                    var properties = pss.GetAdvancedProperties(pdc, Authentication.GetCurrentUser());
                    ViewData["Properties"] = properties;

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                    {
                        boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                    }
                    ViewData["BOProperties"] = boproperties;

                    item = ((ItemBase)(item)).PopulateOne((ItemBase)(item));

                    var DataItems = new Dictionary<long, ItemBase>();

                    DataItems.Add(item.Id, item);

                    return File(CSVExportHelper.ExportCSV(DataItems, properties, true), "application/vnd.ms-excel", item.GetType().Name + ".csv");
                }
                else
                {
                    var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                    ViewData["BOType"] = item.GetType();
                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item.GetType());
                    var properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());
                    ViewData["Properties"] = properties;

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                    {
                        boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                    }
                    ViewData["BOProperties"] = boproperties;

                    var is_search = false;

                    if (!string.IsNullOrEmpty(Request.Form["isFileter"]))
                    {
                        is_search = true;

                        var search_properties = pss.GetSearchProperties(pdc);

                        foreach (AdvancedProperty property in search_properties)
                        {
                            property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property));
                        }
                    }

                    var DataItems = ((ItemBase)(item)).Populate((is_search ? (ItemBase)(item) : null));

                    return File(CSVExportHelper.ExportCSV(DataItems, properties, true), "application/vnd.ms-excel", item.GetType().Name + ".csv");
                }
            }

            return File("", "application/vnd.ms-excel");
        }
    }
}
