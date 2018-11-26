using System.Linq;
using Bowgum.GraphQL.Authentication;
using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Resolver.Query {
    public class TotalNumberOfUsersField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;
        UsersPubSub _pubsub;

        public TotalNumberOfUsersField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _db = db;
            _appSettings = appSettings;
            _pubsub = pubsub;

            Type = typeof(IntGraphType);
            Name = "totalNumberOfUsers";
            Description = "Total number of users";
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            return _db.Users.Count();
        }
    }
}