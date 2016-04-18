using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Entity;
using ContactManager.Services.Infrastructure;
using Microsoft.Extensions.PlatformAbstractions;
using ContactManager.Services.Infrastructure.Repositories;
using ContactManager.Services.Infrastructure.Mappings;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ContactManager.Services
{
    public class Startup
    {
        private static string _applicationPath = string.Empty;

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            
            _applicationPath = appEnv.ApplicationBasePath;
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Add framework services.
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            //var connection = "Server=(localdb)\\MSSQLLocalDB;Database=ContactManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            var connection = Configuration["Data:ContactManagerConnection:ConnectionString"];

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ContactManagerContext>(options =>
                options.UseSqlServer(connection));

            // Repositories
            services.AddScoped<IUserContactRepository, UserContactRepository>();
            services.AddScoped<IContactNoteRepository, ContactNoteRepository>();
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();
            app.UseStaticFiles();

            AutoMapperConfiguration.Configure();

            app.UseMvc(routes => {
                routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            DbInitializer.Initialize(app.ApplicationServices);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
