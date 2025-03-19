using System;
using System.Web.Mvc;
using System.Collections.Generic;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            try
            {
                List<Category> categories = Category.GetAllCategories();
                return View(categories);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving categories: " + ex.Message;
                return View(new List<Category>());
            }
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Category category = Category.GetCategoryByID(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving category: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    category.Insert();
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error creating category: " + ex.Message;
                return View(category);
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Category category = Category.GetCategoryByID(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving category: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    category.Update();
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error updating category: " + ex.Message;
                return View(category);
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Category category = Category.GetCategoryByID(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving category: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Category category = Category.GetCategoryByID(id);
                if (category == null)
                {
                    return HttpNotFound();
                }

                // Check if category has products
                List<Product> products = Product.GetProductsByCategoryID(id);
                if (products.Count > 0)
                {
                    ViewBag.ErrorMessage = "Cannot delete category because it contains products. Remove or reassign the products first.";
                    return View(category);
                }

                category.Delete();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error deleting category: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
} 