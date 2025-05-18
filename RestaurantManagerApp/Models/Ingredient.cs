namespace RestaurantManagerApp.Models
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }


        public virtual ICollection<IngredientInProduct>? IngredientInProducts { get; set; }


        public Ingredient() { }
        public Ingredient(string name)
        {
            Id = Guid.NewGuid();

            Name = name;
        }
    }
}
