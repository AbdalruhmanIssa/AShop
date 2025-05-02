using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace AShop.API.Validation
{
    public class Over18Years(int minimumAge) :ValidationAttribute
    {
        private readonly int minimumAge=  minimumAge;


        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                var today = DateTime.Today;
                int age = today.Year - date.Year;
                if (date.Date > today.AddYears(-age)) age--; // Adjust if birthday hasn't occurred yet this year

                return age >= minimumAge;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be at least {minimumAge} years old";
        }
    }
}

