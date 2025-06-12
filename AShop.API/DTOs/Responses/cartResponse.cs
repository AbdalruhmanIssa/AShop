using AShop.API.Models;

namespace AShop.API.DTOs.Responses
{
    public class cartResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string mainImg { get; set; }
     
    }
}
