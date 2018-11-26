using System.Linq;
using Bowgum.GraphQL.Authentication;
using Bowgum.GraphQL.Schemas.Users.Type.Output;
using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Resolver.Query {
    public class TakeUsersField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;
        UsersPubSub _pubsub;

        public TakeUsersField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _db = db;
            _appSettings = appSettings;
            _pubsub = pubsub;

            Type = typeof(ListGraphType<UserType>);
            Name = "takeUsers";
            Description = "Take users.";
            Arguments = new QueryArguments(
                new QueryArgument<IntGraphType> { Name = "offset", DefaultValue = 0 },
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "count" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            int offset = context.GetArgument<int>("offset");
            int count = context.GetArgument<int>("count");
            var query = _db.Users.Skip(offset).Take(count);
            return query;
        }
    }
}