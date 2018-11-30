using System.Security.Claims;
using Bowgum;
using Bowgum.GraphQL.Authentication;
using Bowgum.GraphQL.Schemas.Users;
using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Resolver.Query {
    public class ValidateTokenField : AuthenticationFieldType<object, object> {
        AppSettings _appSettings;

        public ValidateTokenField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _appSettings = appSettings;

            Type = typeof(BooleanGraphType);
            Name = "validateToken";
            Description = "validate auth token";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "token" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            try {
                var token = context.GetArgument<string>("token");
                JWTTokenValidator.ValidateAndDecode(token, _appSettings.Secret);
                return true;
            }
            catch { }

            return false;
        }
    }
}