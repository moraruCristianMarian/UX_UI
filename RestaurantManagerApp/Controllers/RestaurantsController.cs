using Ganss.Xss;
using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;

namespace RestaurantManagerApp.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly GeoJsonWriter gj;
        private readonly UserManager<AppUser> um;
        private readonly RoleManager<IdentityRole> rm;

        public RestaurantsController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            gj = new GeoJsonWriter();
            um = userManager;
            rm = roleManager;
        }

        public IActionResult Index()
        {
            var restaurants = db.Restaurants;
            ViewBag.Restaurants = restaurants;

            ViewBag.ShowAdminTools = User.IsInRole("Admin");
            return View();
        }

        public IActionResult IndexMap()
        {
            var restaurants = db.Restaurants;
            foreach (Restaurant rest in restaurants)
                if (rest.Geom != null)
                    rest.GeoJSON = gj.Write(rest.Geom);
                else
                    rest.GeoJSON = null;

            ViewBag.Restaurants = restaurants;

            ViewBag.ShowAdminTools = User.IsInRole("Admin");
            return View();
        }

        public IActionResult Show(Guid id)
        {
            try
            {
                Restaurant rest = db.Restaurants.Include("MenuProducts")
                                                .Include("MenuProducts.Product")
                                                .Include("Images")
                                                .Include("Reviews")
                                                .Include("Reviews.User")
                                                .Where(r => r.Id == id)
                                                .First();

                if (db.Users.Find(um.GetUserId(User)) != null)
                {
                    ViewBag.ThisUserId = db.Users.Find(um.GetUserId(User)).Id;
                    ViewBag.ThisUserName = db.Users.Find(um.GetUserId(User)).UserName;
                }
                else
                    ViewBag.ThisUserName = "Conectați-vă pentru a posta o recenzie";

                ViewBag.ShowAdminTools = User.IsInRole("Admin");
                return View(rest);
            }
            catch(InvalidOperationException)
            {
                TempData["message"] = "Restaurantul nu a putut fi găsit.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Show([FromForm] Review review)
        {
            review.PostDate = DateTime.Now;
            review.ReviewedObjectTypeId = 1;
            review.UserId = um.GetUserId(User);

            if (ModelState.IsValid)
            {
                HtmlSanitizer htmlSanitizer = new HtmlSanitizer();
                review.Description = htmlSanitizer.Sanitize(review.Description);

                db.Reviews.Add(review);

                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Recenzia a fost adăugată cu succes.";

                return Redirect("/Restaurants/Show/" + review.ReviewedRestaurantId);
            }
            else
            {
                Restaurant rest;
                try
                {
                    rest = db.Restaurants.Include("MenuProducts")
                                         .Include("MenuProducts.Product")
                                         .Include("Images")
                                         .Include("Reviews")
                                         .Include("Reviews.User")
                                         .Where(r => r.Id == review.ReviewedRestaurantId)
                                         .First();
                }
                catch (InvalidOperationException)
                {
                    TempData["message"] = "Restaurantul nu a putut fi găsit.";
                    return RedirectToAction("Index");
                }

                if (db.Users.Find(um.GetUserId(User)) != null)
                {
                    ViewBag.ThisUserId = db.Users.Find(um.GetUserId(User)).Id;
                    ViewBag.ThisUserName = db.Users.Find(um.GetUserId(User)).UserName;
                }
                else
                    ViewBag.ThisUserName = "Conectați-vă pentru a lăsa o recenzie";

                TempData["message"] = "Recenzie invalidă.";

                ViewBag.ShowAdminTools = User.IsInRole("Admin");
                return View(rest);
            }
        }


        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            Restaurant restaurant = new Restaurant();
            return View(restaurant);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                var htmlSanitizer = new HtmlSanitizer();
                restaurant.Name = htmlSanitizer.Sanitize(restaurant.Name);

                db.Restaurants.Add(restaurant);
                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Restaurant adăugat cu succes.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Restaurant invalid.";
                return View(restaurant);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            try
            {
                Restaurant restaurant = db.Restaurants.Where(r => r.Id == id)
                                                      .First();

                return View(restaurant);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, Restaurant reqRestaurant)
        {
            Restaurant restaurant = db.Restaurants.Find(id);

            if (ModelState.IsValid)
            {
                var htmlSanitizer = new HtmlSanitizer();
                restaurant.Name = htmlSanitizer.Sanitize(reqRestaurant.Name);
                restaurant.OpeningTime = reqRestaurant.OpeningTime;
                restaurant.ClosingTime = reqRestaurant.ClosingTime;

                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Restaurant modificat cu succes.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Restaurant invalid.";
                return View(reqRestaurant);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid id)
        {
            Restaurant restaurant = db.Restaurants.Include("Reviews")
                                                  .Where(r => r.Id == id)
                                                  .First();

            db.Reviews.RemoveRange(restaurant.Reviews);

            db.Restaurants.Remove(restaurant);
            db.SaveChanges();

            TempData["popup-type"] = "success";
            TempData["message"] = "Restaurant șters cu succes.";

            return RedirectToAction("Index");
        }
    }
}
