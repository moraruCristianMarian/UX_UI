namespace RestaurantManagerApp.Models
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public virtual ICollection<MenuProduct>? MenuProducts { get; set; }
        public virtual ICollection<Image>? Images { get; set; }


        public Restaurant(string name, string city)
        {
            Id = Guid.NewGuid();

            Name = name;
            City = city;
        }
    }
}
