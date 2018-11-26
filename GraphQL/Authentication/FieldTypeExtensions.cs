using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Authentication {
    public enum UserRole {
        NORMAL_USER = 1,
        POWER_USER = 2,
        ADMIN_USER = 3
    }

    public static class FieldTypeExtensions {
        static readonly string AUTHORIZATION = "_AUTHORIZATION_";
        static readonly string ROLE = "_ROLE_";

        public static IProvideMetadata Authorization(this IProvideMetadata type) {
            type.Metadata[AUTHORIZATION] = "";
            return type;
        }

        public static bool IsRequiredAuthorization(this IProvideMetadata type) {
            var val = type.GetMetadata<string>(AUTHORIZATION);
            return (val != null);
        }

        public static IProvideMetadata AddRole(this IProvideMetadata type, UserRole role) {
            var roles = type.GetMetadata<List<UserRole>>(ROLE);
            if (roles == null) {
                roles = new List<UserRole>();
                type.Metadata[ROLE] = roles;
            }

            roles.Add(role);
            return type;
        }

        public static List<UserRole> GetRoles(this IProvideMetadata type) {
            return type.GetMetadata<List<UserRole>>(ROLE);
        }

        public static bool HasRole(this IProvideMetadata type, UserRole role) {
            var roles = type.GetMetadata<List<UserRole>>(ROLE);
            if (roles == null)
                return ((int)role >= (int)UserRole.NORMAL_USER);

            return roles.Any(x => (int)role >= (int)x);
        }
    }
}