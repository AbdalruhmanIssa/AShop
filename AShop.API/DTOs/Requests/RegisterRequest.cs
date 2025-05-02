using AShop.API.Models;
using AShop.API.Validation;
using System.ComponentModel.DataAnnotations;

namespace AShop.API.DTOs.Requests
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [MinLength(6)]
        public string UserName { get; set; }
        public string Email { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPasswod { get; set; }
        public Gender Gender { get; set; }
        [Over18Years(18)]
        public DateTime BirthDate { get; set; }
    }
}
