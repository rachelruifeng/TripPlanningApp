using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TheWorld
{
    using System.Net;

    using AutoMapper;

    using Microsoft.AspNet.Authentication.Cookies;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;

    using Newtonsoft.Json.Serialization;

    using TheWorld.Models;
    using TheWorld.Services;
    using TheWorld.ViewModels;

    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public class Startup
    {
        public static IConfiguration Configuration; 

        public Startup(IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                config => {
#if !DEBUG
                     config.Filters.Add(new RequireHttpsAttribute()); 
#endif 
                    })
                .AddJsonOptions(
                    opt =>
                        {
                            opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        });

            services.AddIdentity<WorldUser, IdentityRole>(
                config =>
                    {
                        config.User.RequireUniqueEmail = true;
                        config.Password.RequiredLength = 8;
                        config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                        config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                        {
                            OnRedirectToLogin = context =>
                            {
                                if (context.Request.Path.StartsWithSegments("/api")
                                    && context.Response.StatusCode == (int)HttpStatusCode.OK)
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                }
                                else
                                {
                                    context.Response.Redirect(context.RedirectUri);
                                }
                                return Task.FromResult(0);
                            }
                        };
                    })
                    .AddEntityFrameworkStores<WorldContext>();

            services.AddLogging();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<WorldContext>();

            services.AddScoped<CoordService>();
            services.AddTransient<WorldContextSeedData>();
            services.AddScoped<IWorldRepository, WorldRepository>();

#if DEBUG
            services.AddScoped<IMailService, DebugMailService>();
#else
            services.AddScoped<IMailService, RealMailService>();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug(LogLevel.Warning);

            //app.UseDefaultFiles(); Use MVC to do the routings now. We don't want to servce the index.html as the root accidentally.
            app.UseStaticFiles();

            app.UseIdentity();

            Mapper.Initialize(
                config =>
                    {
                        config.CreateMap<Trip, TripViewModel>().ReverseMap();
                        config.CreateMap<Stop, StopViewModel>().ReverseMap();
                    });

            //Search for the AppController and Index method by default. id is optional
            app.UseMvc(
                config =>
                    {
                        config.MapRoute(
                            name: "Default",
                            template: "{controller}/{action}/{id?}",
                            defaults: new { controller = "App", action = "Index" });
                    });
             await seeder.EnsureSeedDataAsync();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
