using System;
using Bowgum.GraphQL.Authentication;
using Bowgum.Models;
using GraphQL;
using GraphQL.Types;

namespace Bowgum.GraphQL.Schemas.Users.Resolver.Mutation {
    public class DeleteUserField : AuthenticationFieldType<object, object> {
        AppDbContext _db;
        AppSettings _appSettings;
        UsersPubSub _pubsub;

        public DeleteUserField(AppDbContext db, AppSettings appSettings, UsersPubSub pubsub) {
            _db = db;
            _appSettings = appSettings;
            _pubsub = pubsub;

            Name = "deleteUser";
            Type = typeof(StringGraphType);
            Description = "Delete a user";
            Arguments = new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }
            );

            this.Authorization();
            this.AddRole(UserRole.ADMIN_USER);
        }

        protected override object ResolveFunction(ResolveFieldContext<object> context) {
            try {
                var email = context.GetArgument<string>("email");
                var user = _db.Users.Find(email);
                if (user == null) {
                    return "That user doest not exist";
                }

                _pubsub.DeleteUser(user);
                
                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            catch (Exception e) {
                context.Errors.Add(new ExecutionError(e.Message));
            }
            return "That user is deleted";
        }
    }
}