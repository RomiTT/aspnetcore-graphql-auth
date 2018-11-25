using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Mutation {
    public class DeleteUserField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;

        public DeleteUserField(AppDbContext db, AppSettings appSettings) {
            _db = db;
            _appSettings = appSettings;

            Name = "deleteUser";
            Type = typeof(StringGraphType);
            Description = "Delete a user";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            return null;
        }
    }
}