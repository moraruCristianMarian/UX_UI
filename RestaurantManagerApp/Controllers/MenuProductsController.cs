using Ganss.Xss;
using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagerApp.Controllers
{
    public class MenuProductsController : Controller
    {
        private readonly ApplicationDbContext db;

        public MenuProductsController(
            ApplicationDbContext context
            )
        {
            db = context;
        }

        public IActionResult Index()
        {
            var menuProducts = db.MenuProducts;

            ViewBag.MenuProducts = menuProducts.Include("Restaurant")
                                               .Include("Product");

            ViewBag.ShowAdminTools = User.IsInRole("Admin");
            return View();
        }

        [Route("MenuProducts/Show/{pid}/{rid}")]
        public IActionResult Show(Guid pid, Guid rid)
        {
            try
            {
                var menuProduct = db.MenuProducts.Include("Restaurant")
                                                 .Include("Product")
                                                 .Where(mp => (mp.ProductId == pid && mp.RestaurantId == rid))
                                                 .First();

                ViewBag.ShowAdminTools = User.IsInRole("Admin");
                return View(menuProduct);
            }
            catch(InvalidOperationException)
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult New(Guid rid)
        {
            MenuProduct menuProduct = new MenuProduct();
            menuProduct.RestaurantId = rid;

            menuProduct.AllProducts = ListProducts();

            return View(menuProduct);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(MenuProduct menuProduct)
        {
            if (ModelState.IsValid)
            {
                db.MenuProducts.Add(menuProduct);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    menuProduct.AllProducts = ListProducts();
                    TempData["message"] = "Produsul există deja în acest meniu.";
                    return View(menuProduct);
                }

                TempData["popup-type"] = "success";
                TempData["message"] = "Produsul a fost adăugat în meniu cu succes.";
                return Redirect("/Restaurants/Show/" + menuProduct.RestaurantId);
            }
            else
            {
                menuProduct.AllProducts = ListProducts();

                TempData["message"] = "Date invalide - produsul nu a putut fi adăugat în meniu.";

                return View(menuProduct);
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("MenuProducts/Edit/{pid}/{rid}")]
        public IActionResult Edit(Guid pid, Guid rid)
        {
            try
            {
                MenuProduct menuProduct = db.MenuProducts.Include("Restaurant")
                                                         .Include("Product")
                                                         .Where(mp => (mp.ProductId == pid && mp.RestaurantId == rid))
                                                         .First();

                menuProduct.PromotionEndDate -= new TimeSpan(3, 0, 0);
                return View(menuProduct);
            }
            catch(InvalidOperationException)
            {
                TempData["message"] = "Produsul nu a putut fi găsit în acest meniu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("MenuProducts/Edit/{pid}/{rid}")]
        public IActionResult Edit(Guid pid, Guid rid, MenuProduct reqMenuProduct)
        {
            MenuProduct menuProduct = db.MenuProducts.Find(pid, rid);

            if (ModelState.IsValid)
            {
                menuProduct.Discount = reqMenuProduct.Discount;
                menuProduct.PromotionEndDate = reqMenuProduct.PromotionEndDate;

                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Produsul a fost modificat în meniu cu succes.";

                return Redirect("/Restaurants/Show/" + menuProduct.RestaurantId);
            }
            else
            {
                reqMenuProduct.ProductId = menuProduct.ProductId;
                reqMenuProduct.Product = db.Products.Find(menuProduct.ProductId);
                reqMenuProduct.RestaurantId = menuProduct.RestaurantId;
                reqMenuProduct.Restaurant = db.Restaurants.Find(menuProduct.RestaurantId);
                
                TempData["message"] = "Date invalide - produsul nu a putut fi modificat în meniu.";

                return View(reqMenuProduct);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("MenuProducts/Delete/{pid}/{rid}")]
        public ActionResult Delete(Guid pid, Guid rid)
        {
            MenuProduct menuProduct = db.MenuProducts.Where(mp => (mp.ProductId == pid && mp.RestaurantId == rid))
                                                     .First();

            db.MenuProducts.Remove(menuProduct);
            db.SaveChanges();

            TempData["popup-type"] = "success";
            TempData["message"] = "Produsul a fost șters din meniu cu succes.";

            return Redirect("/Restaurants/Show/" + menuProduct.RestaurantId);
        }


        [NonAction]
        public IEnumerable<SelectListItem> ListProducts()
        {
            DbSet<Product> products = db.Products;
            List<Product> sortedProducts = products.OrderBy(p => p.Name).ToList();

            var list = new List<SelectListItem>();
            foreach (Product prod in sortedProducts)
            {
                var li = new SelectListItem();
                li.Value = prod.Id.ToString();
                li.Text = prod.Name;

                list.Add(li);
            }

            return list;
        }
    }
}
