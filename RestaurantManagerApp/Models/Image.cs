using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagerApp.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }


        public Guid RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; }


        public Image(string filePath, string description, DateTime uploadDate, Guid restaurantId)
        {
            Id = Guid.NewGuid();

            FilePath = filePath;
            Description = description;
            UploadDate = uploadDate;
            RestaurantId = restaurantId;
        }
    }
}
