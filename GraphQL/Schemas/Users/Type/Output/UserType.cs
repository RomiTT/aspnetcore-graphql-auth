using Bowgum.Models;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Type.Output {
    public class UserType : ObjectGraphType<User> {
        public UserType() {
            Name = "User";
            Field(x => x.Email, type : typeof(IdGraphType)).Description("The ID of the User.");
            Field(x => x.FirstName).Description("First name of the User.");
            Field(x => x.LastName).Description("Last name of the User.");
            Field(x => x.Role, type : typeof(UserRoleEnumType)).Description("The role of the User.");
            Field(x => x.Token).Description("JWT token");
        }
    }
}