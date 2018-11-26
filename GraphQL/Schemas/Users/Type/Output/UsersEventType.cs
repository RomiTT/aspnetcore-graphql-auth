using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Type.Output {
    public class UsersEventType : ObjectGraphType<UsersEvent> {
        public UsersEventType() {
            Name = "UsersEvent";
            Field(x => x.EventName).Description("Event name");
            Field(x => x.User, type:typeof(UserType)).Description("User.");
        }
    }
}