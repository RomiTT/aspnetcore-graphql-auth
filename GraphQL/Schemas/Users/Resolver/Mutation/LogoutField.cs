using System.Security.Claims;
using Bowgum.GraphQL.Authentication;
using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Resolver.Mutation {
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

            this.Authorization();
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            var userContext = context.UserContext as UserContext;
            var user = _db.Users.Find(userContext.Email);
            if (user == null) {
                return "That user does not exist";
            }

            _pubsub.LogoutUser(user);
            return "Logged out";
        }
    }
}