using System.Security.Claims;
using aspnetcore_graphql_auth.Models;
using Microsoft.AspNetCore.Http;

namespace aspnetcore_graphql_auth.GraphQL {
    public class UserContext {
        public ClaimsPrincipal User { get; set; }
        public HttpContext HttpContext { get; set; }
        public AppDbContext DbContext { get; set; }
    }
}