using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagerApp.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext db;

        public RestaurantsController(
            ApplicationDbContext context
            )
        {
            db = context;
        }

        public IActionResult Index()
        {
            var restaurants = db.Restaurants;
            ViewBag.Restaurants = restaurants;

            return View();
        }
        public IActionResult Show(Guid id)
        {
            Restaurant rest = db.Restaurants.Include("MenuProducts")
                                            .Include("MenuProducts.Product")
                                            .Include("Images")
                                            .Where(r => r.Id == id)
                                            .First();

            return View(rest);
        }
    }
}
