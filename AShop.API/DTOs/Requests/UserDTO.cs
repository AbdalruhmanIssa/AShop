using AShop.API.Models;
using Azure.Identity;

namespace AShop.API.DTOs.Requests
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public String Gender { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
