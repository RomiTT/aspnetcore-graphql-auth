using System;
using System.Linq;
using aspnetcore_graphql_auth.GraphQL.Schemas.Users;
using aspnetcore_graphql_auth.Models;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
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
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.Configure<AppSettings>(appSettingsSection);
            services.AddCors();
            services.AddMvc();
            services.AddCustomDbContext(Configuration);
            services.AddCustomGraphQL(HostingEnvironment);
            services.AddCustomAuthentication(appSettings.Secret);
            services.AddResponseCompression(options => {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            InitializeLogger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseAuthentication();
            app.UseWebSockets();
            app.UseGraphQLWebSockets<UsersSchema>("/graphql");
            app.UseGraphQL<UsersSchema>("/graphql");
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseGraphQLPlayground(new GraphQLPlaygroundOptions() {
                    Path = "/ui/playground"
                });
                app.UseGraphiQLServer(new GraphiQLOptions {
                    GraphiQLPath = "/ui/graphiql",
                        GraphQLEndPoint = "/graphql"
                });
                app.UseGraphQLVoyager(new GraphQLVoyagerOptions() {
                    GraphQLEndPoint = "/graphql",
                        Path = "/ui/voyager"
                });
            }
            app.UseMvc();
        }

        private void InitializeLogger() {
            var loggingSection = Configuration.GetSection("Logging");
            var logLevel = LogEventLevel.Warning;
            try { logLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), loggingSection["Level"]); } catch { }

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
    }
}