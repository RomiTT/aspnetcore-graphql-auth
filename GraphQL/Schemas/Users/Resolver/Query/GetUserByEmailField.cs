using System.Linq;
using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Query {
    public class GetUserByEmailField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;
        UsersPubSub _pubsub;

        public GetUserByEmailField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _db = db;
            _appSettings = appSettings;
            _pubsub = pubsub;

            Type = typeof(StringGraphType);
            Name = "getUserByEmail";
            Description = "Get a user by email";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            var email = context.GetArgument<string>("email");
            var query = _db.Users.Where(u => u.Email == email);
            return query;
        }
    }
}