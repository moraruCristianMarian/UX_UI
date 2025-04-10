using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagerApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductsController(
            ApplicationDbContext context
            )
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Show(Guid id)
        {
            Product prod = db.Products.Include("IngredientInProducts")
                                      .Include("IngredientInProducts.Ingredient")
                                      .Where(p => p.Id == id)
                                      .First();

            return View(prod);
        }
    }
}
