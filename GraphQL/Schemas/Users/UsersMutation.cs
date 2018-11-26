using Bowgum.GraphQL.Schemas.Users.Resolver.Mutation;
using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users {
    public class UsersMutation : ObjectGraphType {
        public UsersMutation(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            AddField(new SignupField(db, appSettings, pubsub));
            AddField(new LoginField(db, appSettings, pubsub));
            AddField(new LogoutField(db, appSettings, pubsub));
            AddField(new DeleteUserField(db, appSettings, pubsub));
        }
    }
}