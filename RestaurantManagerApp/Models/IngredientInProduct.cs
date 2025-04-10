namespace RestaurantManagerApp.Models
{
    public class IngredientInProduct
    {
        public Guid IngredientId { get; set; }
        public Guid ProductId { get; set; }


        public virtual Ingredient? Ingredient { get; set; }
        public virtual Product? Product { get; set; }


        public IngredientInProduct(Guid ingredientId, Guid productId)
        {
            IngredientId = ingredientId;
            ProductId = productId;
        }
    }
}
