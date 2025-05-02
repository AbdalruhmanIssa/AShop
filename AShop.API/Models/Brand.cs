namespace AShop.API.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       public string mainImg { get; set; }
        public ICollection<Product> Products { get;  }    

    }
}
