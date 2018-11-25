using aspnetcore_graphql_auth.GraphQL;
using aspnetcore_graphql_auth.GraphQL.Schemas.Users;
using aspnetcore_graphql_auth.Models;
using GraphQL.Server;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace aspnetcore_graphql_auth {
    public static class CustomServiceCollectionExtensions {
        public static IGraphQLBuilder AddWebSocketListener(this IGraphQLBuilder builder) {
            builder.Services
                .AddTransient<IOperationMessageListener, WebSocketListener>();

            return builder;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration config) {
            // Setup DB Context
            //services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("MemoryDB"));
            services.AddDbContext<AppDbContext>(options => options.UseMySql(config.GetConnectionString("MySQL")));
            return services;
        }

        public static IServiceCollection AddCustomGraphQL(this IServiceCollection services, IHostingEnvironment hostingEnvironment) {
            services.AddGraphQL(options => {
                    options.EnableMetrics = true;
                    options.ExposeExceptions = hostingEnvironment.IsDevelopment();
                })
                .AddUserContextBuilder(httpContext => new UserContext {
                    User = httpContext.User,
                    HttpContext = httpContext
                })
                .AddWebSockets()
                //.AddWebSocketListener()
                .AddDataLoader();

            services.AddSingleton<UsersPubSub>();
            services.AddScoped<UsersSchema>();

            return services;
        }
    }
}