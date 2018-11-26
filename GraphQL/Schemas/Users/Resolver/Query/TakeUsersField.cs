using System.Linq;
using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Query {
    public class TakeUsersField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;

        public TakeUsersField(AppDbContext db, AppSettings appSettings) {
            _db = db;
            _appSettings = appSettings;

            Type = typeof(StringGraphType);
            Name = "takeUsers";
            Description = "Take users.";
            Arguments = new QueryArguments(
                new QueryArgument<IntGraphType> { Name="offset", DefaultValue=0 },
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name="count" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            int offset = context.GetArgument<int>("offset");
            int count = context.GetArgument<int>("count");
            var query = _db.Users.Skip(offset).Take(count);
            return query;
        }
    }
}