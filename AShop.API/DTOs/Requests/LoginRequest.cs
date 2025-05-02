using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AShop.API.DTOs.Requests
{
    public class LoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
