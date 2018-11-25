using aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Mutation;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users {
    public class UsersMutation : ObjectGraphType {
        public UsersMutation(AppDbContext db, AppSettings appSettings) {
            AddField(new SignupField(db, appSettings));
            AddField(new LoginField(db, appSettings));
            AddField(new LogoutField(db, appSettings));
            AddField(new DeleteUserField(db, appSettings));
        }
    }
}