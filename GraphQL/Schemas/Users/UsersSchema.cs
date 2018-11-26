using Bowgum.Models;
using GraphQL.Types;
using Microsoft.Extensions.Options;

namespace Bowgum.GraphQL.Schemas.Users {
    public class UsersSchema : Schema {
        public UsersSchema(AppDbContext db, IOptions<AppSettings> options, UsersPubSub pubsub) {
            Query = new UsersQuery(db, options.Value, pubsub);
            Mutation = new UsersMutation(db, options.Value, pubsub);
            Subscription = new UsersSubscription(pubsub);
        }
    }
}