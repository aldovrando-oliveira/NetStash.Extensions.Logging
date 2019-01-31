using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetStash.Extensions.Logging;

namespace Samples.WebApi
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
            services.AddLogging(config => 
            {
                config.ClearProviders();

                config.Services.Configure<NetStashOptions>(Configuration.GetSection("NetStash"));
                // OR
                // config.Services.Configure<NetStashOptions>(options =>
                // {
                //     options.AppName = "App Test";
                //     options.Host = "tcp.localhost.com.br";
                //     options.Port = 5030;
                //     options.ExtraValues = new Dictionary<string, string>
                //     {
                //         { "Author", "Aldo Oliveira" }
                //     };
                // });

                // Additional Informations
                config.Services.PostConfigure<NetStashOptions>(options =>
                {
                    options.ExtraValues.Add("Environment", Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT"));
                    options.ExtraValues.Add("Version", Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
                });

                config.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddNetStash();   
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
