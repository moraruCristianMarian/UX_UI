using Ganss.Xss;
using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagerApp.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductCategoriesController(
            ApplicationDbContext context
            )
        {
            db = context;
        }

        public IActionResult Index()
        {
            var productCategories = db.ProductCategories;

            ViewBag.ProductCategories = productCategories;

            ViewBag.ShowAdminTools = User.IsInRole("Admin");
            return View();
        }

        public IActionResult Show(Guid id)
        {
            try
            {
                var productCategory = db.ProductCategories.Where(pc => pc.Id == id)
                                               .First();

                ViewBag.ShowAdminTools = User.IsInRole("Admin");
                return View(productCategory);
            }
            catch(InvalidOperationException)
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var htmlSanitizer = new HtmlSanitizer();
                productCategory.Name = htmlSanitizer.Sanitize(productCategory.Name);

                db.ProductCategories.Add(productCategory);
                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Categoria de produse a fost adăugată cu succes.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Categoria de produse nu a putut fi adăugată.";
                return View(productCategory);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            try
            {
                ProductCategory productCategory = db.ProductCategories.Where(pc => pc.Id == id)
                                                                      .First();

                return View(productCategory);
            }
            catch (InvalidOperationException)
            {
                TempData["message"] = "Categoria de produse nu a putut fi găsită.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, ProductCategory reqProductCategory)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);

            if (ModelState.IsValid)
            {
                var htmlSanitizer = new HtmlSanitizer();
                productCategory.Name = htmlSanitizer.Sanitize(reqProductCategory.Name);

                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Categoria de produse a fost modificată cu succes.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Categoria de produse nu a putut fi modificată.";
                return View(reqProductCategory);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid id)
        {
            ProductCategory productCategory = db.ProductCategories.Where(pc => pc.Id == id)
                                                                  .First();

            db.ProductCategories.Remove(productCategory);
            db.SaveChanges();

            TempData["popup-type"] = "success";
            TempData["message"] = "Categoria de produse a fost ștearsă cu succes.";

            return RedirectToAction("Index");
        }
    }
}
