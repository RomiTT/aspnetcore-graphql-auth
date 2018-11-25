using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Mutation {
    public class LogoutField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;

        public LogoutField(AppDbContext db, AppSettings appSettings) {
            _db = db;
            _appSettings = appSettings;

            Name = "logout";
            Type = typeof(StringGraphType);
            Description = "Logout a user.";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            return null;
        }
    }
}