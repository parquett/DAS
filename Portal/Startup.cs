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
            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            //});
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowSpecificOrigins", builder =>
            //    {
            //        builder.WithOrigins("https://trusted-origin.com")
            //               .AllowAnyHeader()
            //               .AllowAnyMethod();
            //    });
            //});
            services.AddSession();

            // Adding Antiforgery with custom options
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "X-CSRF-TOKEN"; // Name of the CSRF cookie
                options.HeaderName = "X-CSRF-TOKEN";  // Token header name for AJAX requests
            });

            //services.AddAuthentication("CookieAuth")
            //        .AddCookie("CookieAuth", options =>
            //        {
            //            options.Cookie.HttpOnly = true;
            //            options.LoginPath = "/Account/Login";
            //            options.AccessDeniedPath = "/Account/AccessDenied";
            //        });

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
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();

            //// Add Content Security Policy (CSP) Middleware
            ////app.Use(async (context, next) =>
            ////{
            ////    context.Response.Headers.Add("Content-Security-Policy",
            ////        "default-src 'self'; " +
            ////        "script-src 'self' https://trusted-scripts.com; " +
            ////        "style-src 'self' https://trusted-styles.com; " +
            ////        "img-src 'self' data:; " +
            ////        "connect-src 'self';");
            ////    await next();
            ////});

            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            //app.UseRouting();
            //app.UseCors("AllowSpecificOrigins");
            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseSession();

            app.UseStatusCodePages();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("Server");
                await next();
            });

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("Content-Security-Policy",
            //        "default-src 'self'; " +
            //        "script-src 'self'; " +
            //        "style-src 'self'; " +
            //        "img-src 'self' data:; " +
            //        "connect-src 'self';");
            //    await next();
            //});

            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

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
