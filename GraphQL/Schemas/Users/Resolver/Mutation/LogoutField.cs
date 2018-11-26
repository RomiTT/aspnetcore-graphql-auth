using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Mutation {
    public class LogoutField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;
        UsersPubSub _pubsub;

        public LogoutField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _db = db;
            _appSettings = appSettings;
            _pubsub = pubsub;

            Name = "logout";
            Type = typeof(StringGraphType);
            Description = "Logout a user.";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            var email = context.GetArgument<string>("email");
            var user = _db.Users.Find(email);
            if (user == null) {
                return "That user does not exist";
            }

            _pubsub.LogoutUser(user);
            return "Logged out";
        }
    }
}