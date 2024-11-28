// This Startup file is based on ASP.NET Core new project templates and is included
// as a starting point for DI registration and HTTP request processing pipeline configuration.
// This file will need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core startup files, see https://docs.microsoft.com/aspnet/core/fundamentals/startup

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SecurityCRM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddSession();
            services.AddMemoryCache();
            services.AddMvc();
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession();  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: null,
            //        template: "{category}/Page{page:int}",
            //        defaults: new { controller = "Product", action = "List" }
            //        );

            //    routes.MapRoute(
            //        name: null,
            //        template: "Page{page:int}",
            //        defaults: new { controller = "Product", action = "List", page = 1 }
            //        );

            //    routes.MapRoute(
            //        name: null,
            //        template: "{category}",
            //        defaults: new { controller = "Product", action = "List", page = 1 }
            //        );

            //    routes.MapRoute(
            //        name: null,
            //        template: "",
            //        defaults: new { controller = "Product", action = "List", page = 1 }
            //        );

            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Product}/{action=List}/{id?}"
            //        );
            //});

            app.UseRouting();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=DashBoard}/{action=Index}");
            });
        }

        private void ConfigureMvcOptions(MvcOptions mvcOptions)
        { 
        }
    }
}
