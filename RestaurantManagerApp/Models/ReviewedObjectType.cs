namespace RestaurantManagerApp.Models
{
    public class ReviewedObjectType
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public ReviewedObjectType(string name)
        {
            Name = name;
        }
    }
}
