using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagerApp.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext db;

        public ImagesController(
            ApplicationDbContext context
            )
        {
            db = context;
        }


        [HttpPost]
        public IActionResult New(IFormFile file, string description, Guid restaurantId)
        {
            Restaurant rest = db.Restaurants
                                .Where(r => r.Id == restaurantId)
                                .First();

            if ((rest == null) || (file == null))
                return Redirect("/Restaurants/Index");

            String fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            String filePath = Path.Combine("wwwroot/img", fileName);

            FileStream fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);

            Image image = new Image(fileName, description, DateTime.Now.ToUniversalTime(), restaurantId);

            db.Images.Add(image);
            db.SaveChanges();

            return Redirect("/Restaurants/Show/" + restaurantId);
        }
    }
}
