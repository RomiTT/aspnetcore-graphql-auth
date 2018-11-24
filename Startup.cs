using System;
using aspnetcore_graphql_auth.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace aspnetcore_graphql_auth {
    public class Startup {
        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetService<AppDbContext>();

            var loggingSection = Configuration.GetSection("Logging");
            var logLevel = LogEventLevel.Warning;
            try { logLevel = (LogEventLevel) Enum.Parse(typeof(LogEventLevel), loggingSection["Level"]); } catch { }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", logLevel)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.RollingFile($"{HostingEnvironment.ContentRootPath}/log/" + "log-{Date}.txt",
                    fileSizeLimitBytes : 100 * 1024 * 1024, // default 1GB
                    shared : true,
                    flushToDiskInterval : TimeSpan.FromSeconds(1))
                .CreateLogger();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.Run(async(context) => {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}