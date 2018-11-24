using aspnetcore_graphql_auth.GraphQL;
using aspnetcore_graphql_auth.Models;
using GraphQL.Server;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetService<AppDbContext>();

            return services;
        }
    }
}