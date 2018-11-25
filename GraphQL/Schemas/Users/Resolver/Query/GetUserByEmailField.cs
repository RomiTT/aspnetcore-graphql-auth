using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Query {
    public class GetUserByEmailField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;

        public GetUserByEmailField(AppDbContext db, AppSettings appSettings) {
            _db = db;
            _appSettings = appSettings;

            Type = typeof(StringGraphType);
            Name = "getUserByEmail";
            Description = "Get a user by email";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            return null;
        }
    }
}