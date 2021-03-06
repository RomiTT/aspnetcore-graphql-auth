using System;
using System.Linq;
using Bowgum.GraphQL.Authentication;
using Bowgum.GraphQL.Schemas.Users.Type.Output;
using Bowgum.Models;
using GraphQL;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Resolver.Mutation {
    public class SignupField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;
        UsersPubSub _pubsub;

        public SignupField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _db = db;
            _appSettings = appSettings;
            _pubsub = pubsub;

            Name = "signup";
            Type = typeof(UserType);
            Description = "Signup a user.";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "firstName" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "lastName" }
            );
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            var email = context.GetArgument<string>("email");
            var password = context.GetArgument<string>("password");
            var firstName = context.GetArgument<string>("firstName");
            var lastName = context.GetArgument<string>("lastName");

            if (_db.Users.Where(u => u.Email == email).Count() > 0) {
                context.Errors.Add(new ExecutionError("That email exists."));
                return null;
            }

            var hashedPassword = Hash.Create(password, _appSettings.Secret);
            var user = new User {
                Email = email,
                Password = hashedPassword,
                FirstName = firstName,
                LastName = lastName,
                Role = (email == "bowgum.kim@gmail.com") ? UserRole.ADMIN_USER : UserRole.NORMAL_USER
            };

            _db.Users.Add(user);
            _db.SaveChanges();
            
            _pubsub.SignupUser(user);

            user.Token = JWTTokenGenerator.Generate(_appSettings.Secret, email, user.Role, DateTime.UtcNow.AddDays(7));
            return user;
        }
    }
}