using System;
using Bowgum.GraphQL.Authentication;
using Bowgum.GraphQL.Schemas.Users.Type.Output;
using Bowgum.Models;
using GraphQL;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Resolver.Mutation {
    public class LoginField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;
        UsersPubSub _pubsub;

        public LoginField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _db = db;
            _appSettings = appSettings;
            _pubsub = pubsub;

            Name = "login";
            Type = typeof(UserType);
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
                context.Errors.Add(new ExecutionError("That user does not exist"));
                return null;
            }

            var hashedPassword = Hash.Create(password, _appSettings.Secret);
            if (hashedPassword != user.Password) {
                context.Errors.Add(new ExecutionError("Invalid password"));
                return null;
            }

            _pubsub.LoginUser(user);

            user.Token = JWTTokenGenerator.Generate(_appSettings.Secret, email, user.Role, DateTime.UtcNow.AddDays(7));
            return user;
        }
    }
}