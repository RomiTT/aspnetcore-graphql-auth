using System.Security.Claims;
using Bowgum.Models;
using Microsoft.AspNetCore.Http;

namespace Bowgum.GraphQL {
    public class UserContext {
        public string Email { get; set; }
        public ClaimsPrincipal User { get; set; }
        public HttpContext HttpContext { get; set; }
    }
}