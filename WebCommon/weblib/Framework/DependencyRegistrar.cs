using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;

namespace Weblib.Framework
{
    using System.Linq;

    using Lib.Configuration;
    using Lib.Infrastructure;
    using Lib.Infrastructure.DependencyManagement;
    using Lib.Plugins;
    using Lib.Tools.Utils;
    using WebLib.UI;

    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerRequest();

            //controllers
            //builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            
            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerRequest();
            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerRequest();
           

        }

        public int Order
        {
            get { return 0; }
        }
    }


    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        /*
static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
{
   return RegistrationBuilder
       .ForDelegate((c, p) =>
       {
           var currentStoreId = c.Resolve<IStoreContext>().CurrentStore.Id;
           //uncomment the code below if you want load settings per store only when you have two stores installed.
           //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
           //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

           //although it's better to connect to your database and execute the following SQL:
           //DELETE FROM [Setting] WHERE [StoreId] > 0
           return c.Resolve<ISettingService>().LoadSetting<TSettings>(currentStoreId);
       })
       .InstancePerHttpRequest()
       .CreateRegistration();
}
*/
        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

}
