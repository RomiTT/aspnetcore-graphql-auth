using System.Security.Claims;
using aspnetcore_graphql_auth.Models;
using Microsoft.AspNetCore.Http;

namespace aspnetcore_graphql_auth.GraphQL {
    public class UserContext {
        public string Email { get; set; }
        public ClaimsPrincipal User { get; set; }
        public HttpContext HttpContext { get; set; }
    }
}