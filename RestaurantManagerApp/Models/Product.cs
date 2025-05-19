using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagerApp.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Cost { get; set; }
        public string? ImageFilePath { get; set; }

        public Guid? ProductCategoryId { get; set; }


        public virtual ProductCategory? ProductCategory { get; set; }
        public virtual ICollection<MenuProduct>? MenuProducts { get; set; }
        public virtual ICollection<IngredientInProduct>? IngredientInProducts { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? AllIngredients { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? AllProductCategories { get; set; }
        [NotMapped]
        public List<Guid>? FormIngredients { get; set; }
        [NotMapped]
        public float? DiscountedCost { get; set; }


        public Product() { }
        public Product(string name, float cost)
        {
            Id = Guid.NewGuid();

            Name = name;
            Cost = cost;
        }
    }
}
