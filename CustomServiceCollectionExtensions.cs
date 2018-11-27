using System.Text;
using Bowgum.GraphQL;
using Bowgum.GraphQL.Schemas.Users;
using Bowgum.Models;
using GraphQL.Server;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bowgum {
    public static class CustomServiceCollectionExtensions {
        public static IGraphQLBuilder AddWebSocketListener(this IGraphQLBuilder builder) {
            builder.Services
                .AddSingleton<IOperationMessageListener, WebSocketListener>();

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
                .AddWebSocketListener()
                .AddDataLoader();

            services.AddSingleton<UsersPubSub>();
            services.AddScoped<UsersSchema>();

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, string secret) {
            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x => {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}