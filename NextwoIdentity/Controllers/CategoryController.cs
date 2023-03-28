using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NextwoIdentity.Data;
using NextwoIdentity.Models;

namespace NextwoIdentity.Controllers
{
    public class CategoryController : Controller
    {

        private NextwoDbContext db;

        public CategoryController(NextwoDbContext _db)
        {


            db = _db;
        }



        public IActionResult CategoryList()
        {
            return View(db.categories);
        }



        [HttpGet]
        public IActionResult CreateCategory()
        {

            ViewBag.categories = new SelectList(db.categories, "CategoryId", "CategoryName");

            return View();
        }


       
        [HttpPost]

        public IActionResult CreateCategory(Category? category)
        {


            if (ModelState.IsValid)
            {
                var cat = db.categories.FirstOrDefault(s => s.CategoryName == category!.CategoryName);

                if (cat == null)
                {

                    db.categories.Add(category!);
                    db.SaveChanges();
                    return RedirectToAction("CategoryList");

                }
                else
                {

                    ViewBag.check = "exists";
                }

            }


            return View(category);


        }
        [HttpGet]

        public IActionResult EditCategory(int? id)
        {

            ViewBag.categories = new SelectList(db.categories, "CategoryId", "CategoryName");

            if (id == null)
            {
                return RedirectToAction("CategoryList");
            }
            var cat = db.categories.Find(id);
            if (cat == null)
            {
                return RedirectToAction("CategoryList");
            }

            return View(cat);

        }

        [HttpPost]

        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                db.categories.Update(category);
                db.SaveChanges();
                return RedirectToAction("CategoryList");


            }
            return View(category);

        }
        [HttpGet]
        public IActionResult DeleteCategory(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("CategoryList");
            }
            var cat = db.categories.Find(id);
            if (cat == null)
            {
                return RedirectToAction("CategoryList");
            }

            return View(cat);



        }
        [HttpPost]
        public IActionResult DeleteCategory(Category category)
        {
            var cat = db.categories.Find(category.CategoryId);
            if (cat == null)
            {
                return RedirectToAction("CategoryList");
            }

            db.categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("CategoryList");
        }
        [HttpGet]
        public IActionResult Details(int? id)

        {

            if (id == null)
            {
                return RedirectToAction("CategoryList");
            }
            var cat = db.categories.Find(id);
            if (cat== null)
            {
                return RedirectToAction("CategoryList");
            }


            return View(cat);

        }
    }
    }
    
