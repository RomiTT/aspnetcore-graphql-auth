using Bowgum.GraphQL.Schemas.Users.Resolver.Query;
using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users {
    public class UsersQuery : ObjectGraphType {
        public UsersQuery(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            AddField(new TotalNumberOfUsersField(db, appSettings, pubsub));
            AddField(new GetUserByEmailField(db, appSettings, pubsub));
            AddField(new TakeUsersField(db, appSettings, pubsub));
        }
    }
}