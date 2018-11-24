using System;
using System.Net;
using System.Security.Claims;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;

namespace aspnetcore_graphql_auth.Authentication {
    public class AuthenticationFieldType<TSourceType, TReturnType> : FieldType {
        public AuthenticationFieldType() {
            Resolver = new FuncFieldResolver<TSourceType, TReturnType>(context => {
                try {
                    if (TryAuth(context))
                        return ResolveFunction(context);
                } catch (Exception e) {
                    context.Errors.Add(new ExecutionError(e.Message));
                }

                return (TReturnType) Convert.ChangeType(null, typeof(TReturnType));
            });
        }

        protected bool TryAuth(ResolveFieldContext<TSourceType> context) {
            if (context.FieldDefinition.RequiredAuthorization()) {
                var httpContext = context.UserContext as HttpContext;
                var user = httpContext.User;
                if (user.Identity.IsAuthenticated == false) {
                    httpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    context.Errors.Add(new ExecutionError("Unauthorized request.") { Code = "401" });
                    return false;
                }

                var emailClaim = user.FindFirst(ClaimTypes.Email);
                if (emailClaim == null) {
                    httpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    context.Errors.Add(new ExecutionError("Unauthorized request.") { Code = "401" });
                    return false;
                }

                var roleClaim = user.FindFirst(ClaimTypes.Role);
                if (roleClaim != null) {
                    if (roleClaim.Value == "admin") {
                        if (context.FieldDefinition.HasRole(UserRole.ADMIN_USER)) {
                            Console.WriteLine($"Admin User: {emailClaim.Value}");
                        } else {
                            httpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                            context.Errors.Add(new ExecutionError("Unauthorized request.") { Code = "401" });
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        protected virtual TReturnType ResolveFunction(ResolveFieldContext<TSourceType> context) {
            throw new NotImplementedException();
        }
    }
}