// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlPanelController.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the ControlPanelController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

    using ApiContracts.Enums;
namespace SecurityCRM.Controllers
{
    using SecurityCRMLib.BusinessObjects;
    using Weblib.Models.Common;
    using System;
    using Lib.Tools.BO;
    using Lib.AdvancedProperties;
    using System.ComponentModel;
    using Lib.Tools.Security;
    using Lib.Tools.Utils;
    using Lib.BusinessObjects;
    using System.Collections.Generic;
    using System.Linq;
    using Lib.Tools.Revisions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http.Extensions;
    using Weblib.Controllers;
    using System.Reflection;
    using Lib.Helpers;
    using lib;
    using Lib.Tools.AdminArea;
    using Microsoft.AspNetCore.Mvc.ViewEngines;

    /// <summary>
    /// The ControlPanel controller.
    /// </summary>
    [Route("ControlPanel")]
    public class ControlPanelController : BaseController
    {
        public ControlPanelController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        public void LoadSideBarMenue()
        {
            Type[] SecurityCRMLib = GetTypesInNamespace(Assembly.Load("SecurityCRMLib"), "SecurityCRMLib.BusinessObjects");
            Type[] lib = GetTypesInNamespace(Assembly.Load("Lib"), "Lib.BusinessObjects");
            Type[] Galex = AdditionalTypes();
            List<Type> List = new List<Type>(SecurityCRMLib.Concat<Type>(lib).Concat<Type>(Galex));
            SessionHelper<string>.Push(SessionItems.Module, "ControlPanel");
            HttpContextHelper.Current.Items["SystemManagement"] = false;

            Dictionary<AdminAreaGroupenum, List<BusinessObject>> finalList = new Dictionary<AdminAreaGroupenum, List<BusinessObject>>();
            foreach (var type in List)
            {
                Lib.AdvancedProperties.BoAttribute boproperties = null;
                if (type.GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                {
                    boproperties = (Lib.AdvancedProperties.BoAttribute)type.GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                }
                if (boproperties != null)
                {
                    if (
                        (boproperties.ModulesAccess != 0 && (boproperties.ModulesAccess & (long)Modulesenum.ControlPanel) == (long)Modulesenum.ControlPanel)
                        &&
                        (boproperties.ReadAccess == 0
                        ||
                        Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.ReadAccess))
                        )
                    {
                        if (!finalList.ContainsKey(boproperties.Group))
                        {
                            finalList.Add(boproperties.Group, new List<BusinessObject>());
                        }
                        finalList[boproperties.Group].Add(new BusinessObject() { Type = type, Properties = boproperties });
                    }
                }
            }

            ViewData["TypeList"] = finalList;


            ViewBag.Title = "Control Panel";
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && (t.BaseType == typeof(ItemBase) || (t.BaseType.BaseType != null && t.BaseType.BaseType == typeof(ItemBase)) || (t.BaseType.BaseType != null && t.BaseType.BaseType.BaseType != null && t.BaseType.BaseType.BaseType == typeof(ItemBase)))).ToArray();
        }

        private Type[] AdditionalTypes()
        {
            Type[] SecurityCRM = GetTypesInNamespace(this.GetType().Assembly, "SecurityCRM.Models.Objects");
            return SecurityCRM;
        }


        [HttpGet]
        [Route("Edit/{BO}/{Namespace}/{BOLink?}/{NamespaceLink?}/{Id?}")]
        public ActionResult Edit(string BO, string Namespace, string BOLink = "ItemBase", string NamespaceLink = "Lib.BusinessObjects", string Id = "")
        {
            if (!Authentication.CheckUser(this.HttpContext, Modulesenum.ControlPanel))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)SecurityCRM.ApiContracts.Enums.Permissions.CPAccess))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            if (string.IsNullOrEmpty(Namespace))
            {
                return new RedirectResult(Lib.Tools.Utils.URLHelper.GetUrl("ControlPanel/DashBoard"));
            }
            var item = Activator.CreateInstance(Type.GetType(Namespace + "." + BO + ", " + Namespace.Split('.')[0], true));

            ViewData["BOType"] = item.GetType();

            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
            {
                boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
            }

            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.ReadAccess))
            {
                return View("NoAccess");
            }

            if (boproperties.ModulesAccess != 0 && (boproperties.ModulesAccess & (long)Modulesenum.ControlPanel) != (long)Modulesenum.ControlPanel)
            {
                return View("NoAccess");
            }

            ViewData["BOLink"] = BOLink;
            ViewData["NamespaceLink"] = System.Net.WebUtility.UrlEncode(NamespaceLink);
            ViewData["Id"] = Id;
            ViewBag.Title = Config.GetConfigValue("SiteNameAbbr") + ": " + boproperties.DisplayName;

            if (!string.IsNullOrEmpty(Id))
            {
                var LinkItem = Activator.CreateInstance(Type.GetType(NamespaceLink + "." + BOLink + ", " + NamespaceLink.Split('.')[0], true));
                ((ItemBase)LinkItem).Id = Convert.ToInt64(Id);
                LinkItem = ((ItemBase)(LinkItem)).PopulateOne((ItemBase)LinkItem, true);
                ViewData["LinkItem"] = LinkItem;

                BoAttribute BOPropertiesLinked = null;
                if (LinkItem.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                {
                    BOPropertiesLinked = (Lib.AdvancedProperties.BoAttribute)LinkItem.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                }
                ViewData["BOPropertiesLinked"] = BOPropertiesLinked;
                ViewData["BOLinkType"] = LinkItem.GetType();
            }
            else
            {
                ViewData["LinkItem"] = null;
            }

            if (boproperties.AllowCreate && Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.CreateAccess))
            {
                var add_button = new ButtonModel();
                add_button.Name = "add_new";
                if (boproperties != null)
                {
                    add_button.Text = Lib.Tools.Utils.Translate.GetTranslatedValue("Add", "FrontEnd", "Add") + " " + boproperties.SingleName;
                }
                else
                {
                    add_button.Text = Lib.Tools.Utils.Translate.GetTranslatedValue("Add", "FrontEnd", "Add") + " " + item.GetType().Name;
                }
                add_button.Class = "btn btn-success btn-add fancybox.ajax";
                var url = "ControlPanel/CreateItem/" + item.GetType().Name + "/" + System.Net.WebUtility.UrlEncode(item.GetType().Namespace);
                if (!string.IsNullOrEmpty(Id))
                {
                    url += "/" + BOLink + "/" + System.Net.WebUtility.UrlEncode(NamespaceLink) + "/" + Id;
                }
                add_button.Href = Lib.Tools.Utils.URLHelper.GetUrl(url);
                add_button.Icon = "plus";
                ViewData["Add_Button"] = add_button;
            }
            else
            {
                ViewData["Add_Button"] = null;
            }

            if (Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.PrintAccess))
            {
                var print_button = new ButtonModel();
                print_button.Name = "print";
                print_button.Text = Lib.Tools.Utils.Translate.GetTranslatedValue("Print", "FrontEnd", "Print");
                print_button.Class = "btn btn-default btn-print";
                print_button.Icon = "print";
                print_button.Action = "do_print_class()";
                ViewData["Print_Button"] = print_button;
            }
            else
            {
                ViewData["Print_Button"] = null;
            }

            if (Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.ExportAccess))
            {
                var export_button = new ButtonModel();
                export_button.Name = "export";
                export_button.Text = Lib.Tools.Utils.Translate.GetTranslatedValue("Export", "FrontEnd", "Export");
                export_button.Class = "btn btn-primary btn-export";
                export_button.Icon = "download";
                ViewData["Export_Button"] = export_button;
            }
            else
            {
                ViewData["Export_Button"] = null;
            }

            if (boproperties.AllowImport && Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.ImportAccess))
            {
                var import_button = new ButtonModel();
                import_button.Name = "import";
                import_button.Text = Lib.Tools.Utils.Translate.GetTranslatedValue("Import", "FrontEnd", "Import");
                import_button.Class = "btn btn-warning btn-import";
                import_button.Icon = "file-excel-o";
                ViewData["Import_Button"] = import_button;
            }
            else
            {
                ViewData["Import_Button"] = null;
            }

            if (boproperties.AllowDeleteAll && Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.DeleteAllAccess))
            {
                var deleteall_button = new ButtonModel();
                deleteall_button.Name = "deleteall";
                deleteall_button.Text = "Delete All";
                deleteall_button.Class = "btn btn-danger btn-deleteall";
                deleteall_button.Icon = "trash-o";
                deleteall_button.Action = "delete_all_from_grid('" + boproperties.DisplayName + "')";
                ViewData["DeleteAll_Button"] = deleteall_button;
            }
            else
            {
                ViewData["DeleteAll_Button"] = null;
            }

            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());
            var advanced_properties = pss.GetAdvancedProperties(pdc, Authentication.GetCurrentUser());
            var search_properties = pss.GetFilterControlProperties(pdc, Authentication.GetCurrentUser());

            ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;
            ViewData["BOProperties"] = boproperties;
            ((ItemBase)item).Id = -1;
            ViewData["New_Item"] = item;

            ViewData["Properties"] = properties;
            ViewData["AdvancedProperties"] = advanced_properties;
            ViewData["SearchProperties"] = search_properties;
            if (search_properties.Count > 0)
            {
                var search_item = Activator.CreateInstance(Type.GetType(Namespace + "." + BO + ", " + Namespace.Split('.')[0], true));
                ViewData["Search_Item"] = search_item;
            }

            LoadSideBarMenue();
            return View();
        }

        [HttpGet]
        [Route("EditItem/{BO}/{Namespace}/{Id}")]
        public ActionResult EditItem(string BO, string Namespace, long Id)
        {
            if (!Authentication.CheckUser(this.HttpContext, Modulesenum.ControlPanel))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)SecurityCRM.ApiContracts.Enums.Permissions.CPAccess))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }

            var item = Activator.CreateInstance(Type.GetType(Namespace + "." + BO + ", " + Namespace.Split('.')[0], true));
            ViewData["BOType"] = item.GetType();
            ViewData["Back_Link"] = Lib.Tools.Utils.URLHelper.GetUrl("ControlPanel/Edit/" + item.GetType().Name + "/" + System.Net.WebUtility.UrlEncode(item.GetType().Namespace));

            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
            {
                boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
            }

            ViewData["BOProperties"] = boproperties;

            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.ReadAccess))
            {
                return View("NoAccess");
            }

            if (boproperties.ModulesAccess != 0 && (boproperties.ModulesAccess & (long)Modulesenum.ControlPanel) != (long)Modulesenum.ControlPanel)
            {
                return View("NoAccess");
            }

            if (!string.IsNullOrEmpty(Request.Query["BOLink"]))
            {

                var BOLink = Request.Query["BOLink"].FirstOrDefault();
                var NamespaceLink = Request.Query["NamespaceLink"].FirstOrDefault();
                var IdLink = Convert.ToInt64(Request.Query["IdLink"].FirstOrDefault());

                var LinkItem = Activator.CreateInstance(Type.GetType(NamespaceLink + "." + BOLink + ", " + NamespaceLink.Split('.')[0], true));
                ((ItemBase)LinkItem).Id = IdLink;
                LinkItem = ((ItemBase)(LinkItem)).PopulateOne((ItemBase)LinkItem, true);
                ViewData["LinkItem"] = LinkItem;

                ViewData["BOLinkType"] = LinkItem.GetType();
                BoAttribute BOPropertiesLinked = null;
                if (LinkItem.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                {
                    BOPropertiesLinked = (Lib.AdvancedProperties.BoAttribute)LinkItem.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                }
                ViewData["BOPropertiesLinked"] = BOPropertiesLinked;

                ViewData["BOLink"] = BOLink;
                ViewData["NamespaceLink"] = System.Net.WebUtility.UrlEncode(NamespaceLink);
                ViewData["Id"] = IdLink.ToString();
                ViewData["Back_Link"] = Lib.Tools.Utils.URLHelper.GetUrl("ControlPanel/Edit/" + item.GetType().Name + "/" + System.Net.WebUtility.UrlEncode(item.GetType().Namespace) + "/" + BOLink + "/" + System.Net.WebUtility.UrlEncode(NamespaceLink) + "/" + IdLink.ToString());
            }
            else
            {
                ViewData["LinkItem"] = null;
            }

            ((ItemBase)item).Id = Id;
            item = ((ItemBase)(item)).PopulateOne((ItemBase)item, true);

            ViewBag.Title = boproperties.SingleName + ": " + ((ItemBase)(item)).GetName();

            if (item != null)
            {
                ViewData["AllowCRUD"] = true;
                var pss = new PropertySorter();
                var pdc = TypeDescriptor.GetProperties(item.GetType());
                var properties = pss.GetAdvancedProperties(pdc, Authentication.GetCurrentUser());

                var PropertiesGroup = new Dictionary<string, List<AdvancedProperty>>();

                foreach (AdvancedProperty property in properties)
                {
                    if (!PropertiesGroup.ContainsKey(property.Common.DisplayGroup))
                        PropertiesGroup.Add(property.Common.DisplayGroup, new List<AdvancedProperty>());

                    PropertiesGroup[property.Common.DisplayGroup].Add(property);
                }

                if (item.GetType() == typeof(SecurityCRMLib.BusinessObjects.User))
                {
                    if (!Authentication.GetCurrentUser().HasAtLeastOnePermission(((SecurityCRMLib.BusinessObjects.User)item).Role.RoleAccessPermission))
                    {
                        ViewData["AllowCRUD"] = false;
                    }
                }

                ViewData["Properties"] = PropertiesGroup;
                ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;

                if (Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.RevisionsAccess) && boproperties.LogRevisions)
                {
                    ViewData["Revisions"] = Revision.LoadRevisions(BO, Id);
                }
                LoadSideBarMenue();
                return View("EditItem", item);
            }
            else
            {
                return View("NoItem", item);
            }
        }

        [HttpGet]
        [Route("CreateItem/{BO}/{Namespace}/{BOLink?}/{NamespaceLink?}/{Id?}")]
        public ActionResult CreateItem(string BO, string Namespace, string BOLink = "ItemBase", string NamespaceLink = "Lib.BusinessObjects", string Id = "")
        {
            if (!Authentication.CheckUser(this.HttpContext, Modulesenum.ControlPanel))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)SecurityCRM.ApiContracts.Enums.Permissions.CPAccess))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }

            var item = Activator.CreateInstance(Type.GetType(Namespace + "." + BO + ", " + Namespace.Split('.')[0], true));
            ViewData["BOType"] = item.GetType();
            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
            {
                boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
            }

            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.ReadAccess) || !Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(boproperties.CreateAccess))
            {
                return View("NoAccess");
            }

            if (boproperties.ModulesAccess != 0 && (boproperties.ModulesAccess & (long)Modulesenum.ControlPanel) != (long)Modulesenum.ControlPanel)
            {
                return View("NoAccess");
            }

            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var properties = pss.GetAdvancedPropertiesForInsert(pdc, Authentication.GetCurrentUser());

            if (!string.IsNullOrEmpty(Id))
            {
                var LinkItem = Activator.CreateInstance(Type.GetType(NamespaceLink + "." + BOLink + ", " + NamespaceLink.Split('.')[0], true));
                ((ItemBase)LinkItem).Id = Convert.ToInt64(Id);
                foreach (AdvancedProperty property in properties)
                {
                    if (
                        (property.Common.EditTemplate == EditTemplates.Parent
                        || property.Common.EditTemplate == EditTemplates.SelectListParent
                        || property.Common.EditTemplate == EditTemplates.DropDownParent)
                        && property.Type == LinkItem.GetType()
                        )
                    {
                        property.PropertyDescriptor.SetValue(item, LinkItem);
                        break;
                    }
                }
            }
            var PropertiesGroup = new Dictionary<string, List<AdvancedProperty>>();

            foreach (AdvancedProperty property in properties)
            {
                if (!PropertiesGroup.ContainsKey(property.Common.DisplayGroup))
                    PropertiesGroup.Add(property.Common.DisplayGroup, new List<AdvancedProperty>());

                PropertiesGroup[property.Common.DisplayGroup].Add(property);
            }

            ViewData["Properties"] = PropertiesGroup;
            ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;
            ViewData["BOProperties"] = boproperties;

            return View("CreateItem", item);
        }

        [HttpGet]
        [Route("Profile/{Id}")]
        public ActionResult Profile(string Id)
        {
            General.TraceWarn("sId:" + Id);
            if (!Authentication.CheckUser(this.HttpContext, Modulesenum.ControlPanel))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)SecurityCRM.ApiContracts.Enums.Permissions.CPAccess))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            var UserId = Convert.ToInt64(Id);
            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)SecurityCRM.ApiContracts.Enums.Permissions.CPAccess) && UserId != Lib.Tools.Security.Authentication.GetCurrentUser().Id)
            {
                return View("NoAccess");
            }
            LoadSideBarMenue();
            General.TraceWarn("UserId:" + UserId.ToString());
            var user = new SecurityCRMLib.BusinessObjects.User(UserId);
            user = (SecurityCRMLib.BusinessObjects.User)user.PopulateOne((ItemBase)user);
            user.Role = (Role)user.Role.PopulateOne((ItemBase)user.Role);
            user.Person = (Lib.BusinessObjects.Person)user.Person.PopulateOne((ItemBase)user.Person);

            ViewData["Revisions"] = Revision.LoadRevisions(user);
            ViewData["BORevisions"] = Revision.LoadRevisions("User", UserId);

            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(user.GetType());
            var properties = pss.GetAdvancedPropertiesForInsert(pdc, Authentication.GetCurrentUser());
            ViewData["Properties"] = properties;

            ViewBag.Title = "Profile: " + user.GetName();

            return View("Profile", user);
        }

        [HttpGet]
        [Route("")]
        public ActionResult DashBoard()
        {
            if (!Authentication.CheckUser(this.HttpContext, Modulesenum.ControlPanel))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)SecurityCRM.ApiContracts.Enums.Permissions.CPAccess))
            {
                return new RedirectResult(Config.GetConfigValue("CPLoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            LoadSideBarMenue();
            return this.View();
        }

    }
}
