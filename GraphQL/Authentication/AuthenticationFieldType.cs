using System;
using System.Net;
using System.Security.Claims;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace aspnetcore_graphql_auth.GraphQL.Authentication {
    public class AuthenticationFieldType<TSourceType, TReturnType> : FieldType {
        public AuthenticationFieldType() {
            Resolver = new FuncFieldResolver<TSourceType, TReturnType>(context => {
                try {
                    if (TryAuth(context))
                        return ResolveFunction(context);
                }
                catch (Exception e) {
                    context.Errors.Add(new ExecutionError(e.Message));
                }

                return (TReturnType)Convert.ChangeType(null, typeof(TReturnType));
            });
        }

        protected bool TryAuth(ResolveFieldContext<TSourceType> context) {
            if (context.FieldDefinition.IsRequiredAuthorization()) {
                var userContext = context.UserContext as UserContext;
                var httpContext = userContext.HttpContext;
                userContext.Email = null;

                var user = httpContext.User;
                if (user.Identity.IsAuthenticated == false) {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Errors.Add(new ExecutionError("Unauthorized request.") { Code = "401" });
                    return false;
                }

                var emailClaim = user.FindFirst(ClaimTypes.Email);
                if (emailClaim == null) {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Errors.Add(new ExecutionError("Unauthorized request.") { Code = "401" });
                    return false;
                }

                var roleClaim = user.FindFirst(ClaimTypes.Role);
                if (roleClaim != null) {
                    var role = (UserRole)Enum.Parse(typeof(UserRole), roleClaim.Value);
                    if (context.FieldDefinition.HasRole(role)) {
                        Log.Logger.Information($"{roleClaim.Value}: {emailClaim.Value}");
                    }
                    else {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Errors.Add(new ExecutionError("Unauthorized request.") { Code = "401" });
                        return false;
                    }
                }

                userContext.Email = emailClaim.Value;
            }

            return true;
        }

        protected virtual TReturnType ResolveFunction(ResolveFieldContext<TSourceType> context) {
            throw new NotImplementedException();
        }
    }
}