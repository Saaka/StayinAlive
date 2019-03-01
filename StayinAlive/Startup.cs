using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using StayinAlive.Filters;
using System;

namespace StayinAlive
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(o => o.Filters.Add<CustomExceptionFilterAttribute>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddLogging()
                .AddHangfire(c =>
                {
                    c.UseSqlServerStorage(Configuration["DbSettings:ConnectionString"],
                        new Hangfire.SqlServer.SqlServerStorageOptions
                        {
                            SchemaName = "StayinAliveHangFire"
                        });
                    c.UseColouredConsoleLogProvider();
                })
                .AddTransient<IRestClient, RestClient>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHangfireServer()
                .UseHangfireDashboard()
                .UseMvc();

            RecurringJob.AddOrUpdate<Alive.StayAliveJob>((j) => j.Run(), Cron.MinuteInterval(10), TimeZoneInfo.Utc);
        }
    }
}
