using System;
using aspnetcore_graphql_auth.GraphQL.Authentication;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Type.Output {
    public class UserRoleEnumType : EnumerationGraphType {
        public UserRoleEnumType() {
            Name = "UserRole";
            AddValue(UserRole.NORMAL_USER.ToString(), "Normal user", UserRole.NORMAL_USER);
            AddValue(UserRole.POWER_USER.ToString(), "Power user", UserRole.POWER_USER);
            AddValue(UserRole.ADMIN_USER.ToString(), "Admin user", UserRole.ADMIN_USER);
        }
    }
}