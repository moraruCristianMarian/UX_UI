using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerApp.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string Description { get; set; }
        [Range(1, 5, ErrorMessage = "Recenzia trebuie să acorde între 1 și 5 stele!")]
        public int Rating { get; set; }
        public virtual AppUser? User { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime? EditDate { get; set; }


        public int ReviewedObjectTypeId { get; set; }
        public virtual ReviewedObjectType? ReviewedObjectType { get; set; }

        public Guid? ReviewedRestaurantId { get; set; }
        public Guid? ReviewedProductId { get; set; }

        public virtual Restaurant? ReviewedRestaurant { get; set; }
        public virtual Product? ReviewedProduct { get; set; }
    }
}
