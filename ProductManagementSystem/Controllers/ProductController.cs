using System;
using System.Web.Mvc;
using System.Collections.Generic;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            try
            {
                List<Product> products = Product.GetAllProducts();
                return View(products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving products: " + ex.Message;
                return View(new List<Product>());
            }
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Product product = Product.GetProductByID(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving product: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Categories = Category.GetAllCategories();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading categories: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.Insert();
                    return RedirectToAction("Index");
                }
                
                ViewBag.Categories = Category.GetAllCategories();
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error creating product: " + ex.Message;
                ViewBag.Categories = Category.GetAllCategories();
                return View(product);
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Product product = Product.GetProductByID(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                
                ViewBag.Categories = Category.GetAllCategories();
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving product: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.Update();
                    return RedirectToAction("Index");
                }
                
                ViewBag.Categories = Category.GetAllCategories();
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error updating product: " + ex.Message;
                ViewBag.Categories = Category.GetAllCategories();
                return View(product);
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Product product = Product.GetProductByID(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error retrieving product: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Product product = Product.GetProductByID(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                
                product.Delete();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error deleting product: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
} 