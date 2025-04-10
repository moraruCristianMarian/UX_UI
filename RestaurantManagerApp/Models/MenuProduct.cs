namespace RestaurantManagerApp.Models
{
    public class MenuProduct
    {
        public Guid ProductId { get; set; }
        public Guid RestaurantId { get; set; }
        public float Discount { get; set; }
        public DateTime PromotionEndDate { get; set; }


        public virtual Product? Product { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

        public MenuProduct(Guid productId, Guid restaurantId, float discount, DateTime promotionEndDate)
        {
            ProductId = productId;
            RestaurantId = restaurantId;
            Discount = discount;
            PromotionEndDate = promotionEndDate;
        }
    }
}
