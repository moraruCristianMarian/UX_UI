namespace RestaurantManagerApp.Models
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }


        public virtual ICollection<Product>? Products { get; set; }


        public ProductCategory() { }
        public ProductCategory(string name)
        {
            Id = Guid.NewGuid();

            Name = name;
        }
    }
}
