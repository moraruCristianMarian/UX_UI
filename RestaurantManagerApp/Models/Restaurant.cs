using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagerApp.Models
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }
        public Polygon? Geom { get; set; }

        [NotMapped]
        public string? GeoJSON { get; set; }

        public virtual ICollection<MenuProduct>? MenuProducts { get; set; }
        public virtual ICollection<Image>? Images { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }


        public Restaurant() { }
        public Restaurant(string name, TimeSpan openingTime, TimeSpan closingTime, Polygon geom)
        {
            Id = Guid.NewGuid();

            Name = name;
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            Geom = geom;
        }
    }
}
