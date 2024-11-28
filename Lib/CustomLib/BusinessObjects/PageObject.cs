// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageObject.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The Contact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMLib.BusinessObjects
{
    using System;

    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Tools.BO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Reflection;
    using Lib.Tools.AdminArea;

    /// <summary>
    /// The Page.
    /// </summary>
    public class PageObject : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PageObject"/> class.
        /// </summary>
        public PageObject()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageObject"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public PageObject(long id)
            : base(id)
        {
        }
        #endregion

        public override Dictionary<long, ItemBase> Populate(ItemBase item = null,
                                                                SqlConnection conn = null,
                                                                bool sortByName = false,
                                                                string AdvancedFilter = "",
                                                                bool ShowCanceled = false,
                                                                Lib.BusinessObjects.User sUser = null,
                                                                bool ignoreQueryFilter = false)
        {
            var items = new Dictionary<long, ItemBase>();
            Type[] typelist = GetTypesInNamespace(Assembly.Load("SecurityCRM"), "SecurityCRM.Models.Objects");
            long ordinal = 0;
            foreach (var type in typelist)
            {
                items.Add(ordinal, new PageObject() { Type = type });
                ordinal++;
            }
            typelist = GetTypesInNamespace(Assembly.Load("SecurityCRM"), "SecurityCRM.Models.Print");
            foreach (var type in typelist)
            {
                items.Add(ordinal, new PageObject() { Type = type });
                ordinal++;
            }
            typelist = GetTypesInNamespace(Assembly.Load("SecurityCRM"), "SecurityCRM.Models.Reports");
            foreach (var type in typelist)
            {
                items.Add(ordinal, new PageObject() { Type = type });
                ordinal++;
            }
            return items;
        }
        public override Dictionary<long, ItemBase> Populate(List<SortParameter> SortParameters)
        {
            return Populate();
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && (t.BaseType == typeof(ModelBase) || (t.BaseType != null && t.BaseType.BaseType == typeof(ModelBase)) || (t.BaseType != null && t.BaseType.BaseType != null && t.BaseType.BaseType.BaseType == typeof(ModelBase)))).ToArray();
        }

        public override string GetName()
        {
            return Type!=null?Type.Name:"";
        }

        public override Object GetId()
        {
            return Type != null ? Type.FullName : "";
        }

        public override void SetId(object Id)
        {   
            var pages = this.Populate();
            if(pages.Values.Any(p => ((PageObject)p).Type.FullName == Id.ToString()))
                Type = ((PageObject)pages.Values.First(p => ((PageObject)p).Type.FullName == Id.ToString())).Type;
        }

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Name { get { return Type != null ? Type.FullName : ""; } }

        public Type Type { get; set; }

        #endregion
    }
}