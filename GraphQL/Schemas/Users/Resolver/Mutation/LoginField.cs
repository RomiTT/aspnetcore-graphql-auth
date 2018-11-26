using System;
using aspnetcore_graphql_auth.GraphQL.Authentication;
using aspnetcore_graphql_auth.Models;
using GraphQL;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users.Resolver.Mutation {
    public class LoginField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;
        UsersPubSub _pubsub;

        public LoginField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _db = db;
            _appSettings = appSettings;
            _pubsub = pubsub;

            Name = "login";
            Type = typeof(StringGraphType);
            Description = "Login a user.";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            var email = context.GetArgument<string>("email");
            var password = context.GetArgument<string>("password");

            var user = _db.Users.Find(email);
            if (user == null) {
                context.Errors.Add(new ExecutionError("That user does not exist") { Code = "LOGIN_FAILED" });
                return null;
            }

            var hashedPassword = Hash.Create(password, _appSettings.Secret);
            if (hashedPassword != user.Password) {
                context.Errors.Add(new ExecutionError("Invalid password") { Code = "LOGIN_FAILED" });
                return null;
            }

            _pubsub.LoginUser(user);

            var token = JWTTokenGenerator.Generate(_appSettings.Secret, email, user.Role, DateTime.UtcNow.AddDays(7));
            return token;
        }
    }
}