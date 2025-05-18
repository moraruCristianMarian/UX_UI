using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagerApp.Models
{
    public class MenuProduct
    {
        public Guid ProductId { get; set; }
        public Guid RestaurantId { get; set; }
        [Range(0.0, 1.0)]
        public float Discount { get; set; }
        public DateTime? PromotionEndDate { get; set; }


        public virtual Product? Product { get; set; }
        public virtual Restaurant? Restaurant { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? AllProducts { get; set; }
        [NotMapped]
        public string? DiscountTimeLeft {  get; set; }


        public MenuProduct() { }
        public MenuProduct(Guid productId, Guid restaurantId, float discount, DateTime promotionEndDate)
        {
            ProductId = productId;
            RestaurantId = restaurantId;
            Discount = discount;
            PromotionEndDate = promotionEndDate;
        }
    }
}
