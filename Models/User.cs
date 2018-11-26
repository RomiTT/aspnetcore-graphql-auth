using System.ComponentModel.DataAnnotations;
using Bowgum.GraphQL.Authentication;

namespace Bowgum.Models {
    public class User {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        //public string Token { get; set; }
    }
}
