using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Type.Output {
    public class UsersEventType : ObjectGraphType<UsersEvent> {
        public UsersEventType() {
            Name = "UsersEvent";
            Field(x => x.EventName).Description("Event name");
            Field(x => x.User, type:typeof(UserType)).Description("User.");
        }
    }
}