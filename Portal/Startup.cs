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
using Microsoft.AspNetCore.Http;
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

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            // Adding Antiforgery with custom options
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "X-CSRF-TOKEN"; // Name of the CSRF cookie
                options.Cookie.HttpOnly = true;      // Add HttpOnly
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Add Secure
                options.Cookie.SameSite = SameSiteMode.Strict; // Add SameSite
                options.HeaderName = "X-CSRF-TOKEN";  // Token header name for AJAX requests
            });

            services.AddHsts(options =>
            {
                options.Preload = true; // Указывает браузеру предварительно загрузить настройки HSTS
                options.IncludeSubDomains = true; // Применяет HSTS ко всем поддоменам
                options.MaxAge = TimeSpan.FromDays(365); // Устанавливает время действия политики (например, 1 год)
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
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AspNetCore.Session"; // Cookie ame for session
                options.Cookie.HttpOnly = true;             // Add HttpOnly
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Add Secure
                options.Cookie.SameSite = SameSiteMode.Strict; // Add SameSite
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Configure method started..."); // Debugging
            Console.WriteLine($"Current environment: {env.EnvironmentName}"); // Log environment

            if (!env.IsDevelopment())
            {
                Console.WriteLine("Applying HSTS..."); // Debugging
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            Console.WriteLine("HTTPS redirection enabled."); // Debugging

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
          
            app.UseResponseCompression();

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

            //app.UseCookiePolicy();
            //app.UseCors("AllowSpecificOrigins");
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseStatusCodePages();

            app.Use(async (context, next) =>
            {

                 context.Response.Headers.Add("Content-Security-Policy",
                   "default-src 'self'; " +
                    "script-src 'self' https://cdnjs.cloudflare.com http://ckeditor.com 'unsafe-inline'; " +
                    "style-src 'self' https://fonts.googleapis.com https://cdn.jsdelivr.net 'unsafe-inline'; " +
                    "font-src 'self' https://fonts.gstatic.com; " +
                    //"img-src 'self' data:; " +
                    "frame-ancestors 'self'; " +
                    "object-src 'none'; " +
                    "base-uri 'self'; " +
                    "form-action 'self';");

                if (context.Request.Path.StartsWithSegments("/_framework/aspnetcore-browser-refresh.js"))
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                context.Response.Headers.Remove("Server");

                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                //context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self'");

                await next();
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                    context.Context.Response.Headers["Cache-Control"] = "public,max-age=600";
                }
            });

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
