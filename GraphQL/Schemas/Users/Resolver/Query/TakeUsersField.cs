using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Query {
    public class TakeUsersField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;

        public TakeUsersField(AppDbContext db, AppSettings appSettings) {
            _db = db;
            _appSettings = appSettings;

            Type = typeof(StringGraphType);
            Name = "takeUsers";
            Description = "Take users.";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "count" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            return null;
        }
    }
}