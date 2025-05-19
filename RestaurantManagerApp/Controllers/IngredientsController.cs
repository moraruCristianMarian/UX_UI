using Ganss.Xss;
using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagerApp.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly ApplicationDbContext db;

        public IngredientsController(
            ApplicationDbContext context
            )
        {
            db = context;
        }

        public IActionResult Index()
        {
            var ingredients = db.Ingredients;

            ViewBag.Ingredients = ingredients;

            ViewBag.ShowAdminTools = User.IsInRole("Admin");
            return View();
        }

        public IActionResult Show(Guid id)
        {
            try
            {
                var ingredient = db.Ingredients.Where(ing => ing.Id == id)
                                               .First();

                ViewBag.ShowAdminTools = User.IsInRole("Admin");
                return View(ingredient);
            }
            catch(InvalidOperationException)
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            Ingredient ingredient = new Ingredient();
            return View(ingredient);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                var htmlSanitizer = new HtmlSanitizer();
                ingredient.Name = htmlSanitizer.Sanitize(ingredient.Name);

                db.Ingredients.Add(ingredient);
                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Ingredientul a fost adăugat cu succes.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Ingredientul nu a putut fi adăugat.";
                return View(ingredient);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            try
            {
                Ingredient ingredient = db.Ingredients.Where(ing => ing.Id == id)
                                                      .First();

                return View(ingredient);
            }
            catch (InvalidOperationException)
            {
                TempData["message"] = "Ingredientul nu a putut fi găsit.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, Ingredient reqIngredient)
        {
            Ingredient ingredient = db.Ingredients.Find(id);

            if (ModelState.IsValid)
            {
                var htmlSanitizer = new HtmlSanitizer();
                ingredient.Name = htmlSanitizer.Sanitize(reqIngredient.Name);

                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Ingredientul a fost modificat cu succes.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Ingredientul nu a putut fi modificat.";
                return View(reqIngredient);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid id)
        {
            Ingredient ingredient = db.Ingredients.Where(ing => ing.Id == id)
                                                  .First();

            db.Ingredients.Remove(ingredient);
            db.SaveChanges();

            TempData["popup-type"] = "success";
            TempData["message"] = "Ingredientul a fost șters cu succes.";

            return RedirectToAction("Index");
        }
    }
}
