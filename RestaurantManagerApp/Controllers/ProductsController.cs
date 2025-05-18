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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<AppUser> um;
        private readonly RoleManager<IdentityRole> rm;

        public ProductsController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            um = userManager;
            rm = roleManager;
        }

        public IActionResult Index()
        {
            var products = db.Products.Include("ProductCategory");

            ViewBag.Products = products;

            ViewBag.ShowAdminTools = User.IsInRole("Admin");
            return View();
        }
        public IActionResult Show(Guid id)
        {
            try
            {
                Product prod = db.Products.Include("IngredientInProducts")
                                          .Include("IngredientInProducts.Ingredient")
                                          .Include("ProductCategory")
                                          .Include("Reviews")
                                          .Include("Reviews.User")
                                          .Where(p => p.Id == id)
                                          .First();

                if (db.Users.Find(um.GetUserId(User)) != null)
                {
                    ViewBag.ThisUserId = db.Users.Find(um.GetUserId(User)).Id;
                    ViewBag.ThisUserName = db.Users.Find(um.GetUserId(User)).UserName;
                }
                else
                    ViewBag.ThisUserName = "Conectați-vă pentru a posta o recenzie";

                ViewBag.ShowAdminTools = User.IsInRole("Admin");
                return View(prod);
            }
            catch (InvalidOperationException)
            {
                TempData["message"] = "Produsul nu a putut fi găsit.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Show([FromForm] Review review)
        {
            review.PostDate = DateTime.Now;
            review.ReviewedObjectTypeId = 2;
            review.UserId = um.GetUserId(User);

            if (ModelState.IsValid)
            {
                HtmlSanitizer htmlSanitizer = new HtmlSanitizer();
                review.Description = htmlSanitizer.Sanitize(review.Description);

                db.Reviews.Add(review);

                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Recenzia a fost adăugată cu succes.";

                return Redirect("/Products/Show/" + review.ReviewedProductId);
            }
            else
            {
                Product prod;
                try
                {
                    prod = db.Products.Include("IngredientInProducts")
                                      .Include("IngredientInProducts.Ingredient")
                                      .Include("ProductCategory")
                                      .Include("Reviews")
                                      .Include("Reviews.User")
                                      .Where(p => p.Id == review.ReviewedProductId)
                                      .First();
                }
                catch (InvalidOperationException)
                {
                    TempData["message"] = "Produsul nu a putut fi găsit.";
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
                return View(prod);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            Product product = new Product();

            product.AllProductCategories = ListProductCategories();
            product.AllIngredients = ListIngredients();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(Product product, IFormFile file)
        {
            if (file == null)
            {
                TempData["message"] = "Fișierul imaginii nu a fost găsit.";
                return Redirect("/Products/Index");
            }

            String fileName = product.Name + "_";
            fileName += Guid.NewGuid().ToString().Substring(0, 8);
            fileName += Path.GetExtension(file.FileName);

            String filePath = Path.Combine("wwwroot/img", fileName);
            product.ImageFilePath = fileName;

            if (ModelState.IsValid)
            {
                var htmlSanitizer = new HtmlSanitizer();
                product.Name = htmlSanitizer.Sanitize(product.Name);

                db.Products.Add(product);

                if ((product.FormIngredients != null) && (product.FormIngredients.Count > 0))
                {
                    try
                    {
                        foreach (var ingredientId in product.FormIngredients)
                        {
                            IngredientInProduct iip = new IngredientInProduct();
                            iip.ProductId = product.Id;
                            iip.IngredientId = ingredientId;

                            db.IngredientInProducts.Add(iip);
                        }
                    }
                    catch(InvalidOperationException)
                    {
                        TempData["message"] = "Produs invalid - verificați ingredientele adăugate.";

                        product.AllProductCategories = ListProductCategories();
                        product.AllIngredients = ListIngredients();
                        return View(product);
                    }
                }

                TempData["popup-type"] = "success";
                TempData["message"] = "Produsul a fost adăugat cu succes.";

                FileStream fs = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fs);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                product.AllProductCategories = ListProductCategories();
                product.AllIngredients = ListIngredients();
                return View(product);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            try
            {
                Product product = db.Products.Include("IngredientInProducts")
                                             .Include("IngredientInProducts.Ingredient")
                                             .Include("ProductCategory")
                                             .Where(p => p.Id == id)
                                             .First();

                product.AllProductCategories = ListProductCategories();
                product.AllIngredients = ListIngredients();

                return View(product);
            }
            catch (InvalidOperationException)
            {
                TempData["message"] = "Produsul nu a putut fi găsit.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, Product reqProduct)
        {
            Product product = db.Products.Include("IngredientInProducts")
                                         .Include("IngredientInProducts.Ingredient")
                                         .Include("ProductCategory")
                                         .Where(p => p.Id == id)
                                         .First();

            if (ModelState.IsValid)
            {
                var htmlSanitizer = new HtmlSanitizer();
                product.Name = htmlSanitizer.Sanitize(reqProduct.Name);
                product.Cost = reqProduct.Cost;

                db.IngredientInProducts.RemoveRange(product.IngredientInProducts);
                if ((reqProduct.FormIngredients != null) && (reqProduct.FormIngredients.Count > 0))
                {
                    try
                    {
                        foreach (var ingredientId in reqProduct.FormIngredients)
                        {
                            IngredientInProduct iip = new IngredientInProduct();
                            iip.ProductId = reqProduct.Id;
                            iip.IngredientId = ingredientId;

                            try
                            {
                                db.IngredientInProducts.Add(iip);
                            }
                            catch (InvalidOperationException)
                            {
                                continue;
                            }
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        TempData["message"] = "Produs invalid - verificați ingredientele incluse.";
                        product.AllProductCategories = ListProductCategories();
                        product.AllIngredients = ListIngredients();
                        return View(product);
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch(DbUpdateException)
                {
                    TempData["message"] = "Produs invalid.";
                    product.AllProductCategories = ListProductCategories();
                    product.AllIngredients = ListIngredients();
                    return View(product);
                }

                TempData["popup-type"] = "success";
                TempData["message"] = "Produsul a fost modificat cu succes.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Produs invalid.";
                product.AllProductCategories = ListProductCategories();
                product.AllIngredients = ListIngredients();
                return View(product);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid id)
        {
            Product product = db.Products.Include("Reviews")
                                         .Where(p => p.Id == id)
                                         .First();

            db.Reviews.RemoveRange(product.Reviews);

            db.Products.Remove(product);
            db.SaveChanges();

            TempData["popup-type"] = "success";
            TempData["message"] = "Produsul a fost șters cu succes.";

            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> ListIngredients()
        {
            DbSet<Ingredient> ingredients = db.Ingredients;
            List<Ingredient> sortedIngredients = ingredients.OrderBy(i => i.Name).ToList();

            var list = new List<SelectListItem>();
            foreach (Ingredient ing in sortedIngredients)
            {
                var li = new SelectListItem();
                li.Value = ing.Id.ToString();
                li.Text = ing.Name;

                list.Add(li);
             }

            return list;
        }

        [NonAction]
        public IEnumerable<SelectListItem> ListProductCategories()
        {
            DbSet<ProductCategory> productCategories = db.ProductCategories;
            List<ProductCategory> sortedProductCategories = productCategories.OrderBy(i => i.Name).ToList();

            var list = new List<SelectListItem>();
            foreach (ProductCategory pc in sortedProductCategories)
            {
                var li = new SelectListItem();
                li.Value = pc.Id.ToString();
                li.Text = pc.Name;

                list.Add(li);
            }

            return list;
        }
    }
}
