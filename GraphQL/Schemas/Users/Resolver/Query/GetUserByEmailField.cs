using System.Linq;
using Bowgum.GraphQL.Authentication;
using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Resolver.Query {
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

            this.Authorization();
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            var email = context.GetArgument<string>("email");
            var query = _db.Users.Where(u => u.Email == email);
            return query;
        }
    }
}