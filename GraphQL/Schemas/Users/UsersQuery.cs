using aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Query;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users {
    public class UsersQuery : ObjectGraphType {
        public UsersQuery(AppDbContext db, AppSettings appSettings) {
            AddField(new GetUserByEmailField(db, appSettings));
            AddField(new TakeUsersField(db, appSettings));
        }
    }
}