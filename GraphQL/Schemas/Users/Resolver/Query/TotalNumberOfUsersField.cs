using System.Linq;
using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Query {
    public class TotalNumberOfUsersField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;

        public TotalNumberOfUsersField(AppDbContext db, AppSettings appSettings) {
            _db = db;
            _appSettings = appSettings;

            Type = typeof(IntGraphType);
            Name = "totalNumberOfUsers";
            Description = "Total number of users";
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            return _db.Users.Count();
        }
    }
}