using Ganss.Xss;
using RestaurantManagerApp.Data;
using RestaurantManagerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO;

namespace RestaurantManagerApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<AppUser> um;
        private readonly RoleManager<IdentityRole> rm;

        public ReviewsController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            um = userManager;
            rm = roleManager;
        }


        [Authorize]
        public IActionResult Edit(Guid id)
        {
            try
            {
                Review review = db.Reviews.Where(r => r.Id == id)
                                          .First();

                if (review.ReviewedRestaurantId != null)
                {
                    ViewBag.ReviewedObjectId = review.ReviewedRestaurantId;
                    ViewBag.IsRestaurantReview = true;
                }
                else
                {
                    ViewBag.ReviewedObjectId = review.ReviewedProductId;
                    ViewBag.IsRestaurantReview = false;
                }
                ViewBag.FormActionUrl = String.Format("/Reviews/Edit/{0}", review.Id);

                TempData["popup-type"] = "success";
                TempData["message"] = "Recenzia a fost adăugată cu succes.";
                return View(review);
            }
            catch (InvalidOperationException)
            {
                TempData["message"] = "Recenzia nu a putut fi adăugată.";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Guid id, Review reqReview)
        {
            try
            {
                Review review = db.Reviews.Where(r => r.Id == id)
                                          .First();
                review.EditDate = DateTime.Now;

                if ((review.UserId != um.GetUserId(User)) && (!User.IsInRole("Admin")))
                    return View(reqReview);

                if (ModelState.IsValid)
                {
                    var htmlSanitizer = new HtmlSanitizer();
                    review.Description = htmlSanitizer.Sanitize(reqReview.Description);
                    review.Rating = reqReview.Rating;

                    db.SaveChanges();

                    TempData["popup-type"] = "success";
                    TempData["message"] = "Recenzie modificată cu succes.";
                }
                else
                {
                    TempData["popup-type"] = "danger";
                    TempData["message"] = "Recenzie invalidă.";
                }
                if (review.ReviewedObjectTypeId == 1)
                    return Redirect("/Restaurants/Show/" + reqReview.ReviewedRestaurantId);
                if (review.ReviewedObjectTypeId == 2)
                    return Redirect("/Products/Show/" + reqReview.ReviewedProductId);

                return RedirectToAction("Index", "Home");
            }
            catch (RuntimeBinderException)
            {
                TempData["message"] = "Recenzie invalidă.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            Review rev = db.Reviews.Find(id);

            if (rev.UserId == um.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(rev);
                db.SaveChanges();

                TempData["popup-type"] = "success";
                TempData["message"] = "Recenzie ștearsă cu succes.";

                if (rev.ReviewedObjectTypeId == 1)
                    return Redirect("/Restaurants/Show/" + rev.ReviewedRestaurantId);
                if (rev.ReviewedObjectTypeId == 2)
                    return Redirect("/Products/Show/" + rev.ReviewedProductId);
            }

            TempData["message"] = "Recenzia nu a putut fi ștearsă.";
            return RedirectToAction("Index", "Home");
        }
    }
}
