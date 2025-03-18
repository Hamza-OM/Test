using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using ProductManagementSystem.Models;

namespace ProductManagementSystem
{
    public partial class Products : System.Web.UI.Page
    {
        public string CurrentAction { get; private set; }
        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }
        public List<Product> AllProducts { get; private set; }
        public List<Category> AllCategories { get; private set; }
        public Product CurrentProduct { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the current action from the query string
            CurrentAction = Request.QueryString["action"] ?? "list";
            
            // Load categories for dropdown
            LoadCategories();
            
            if (IsPostBack)
            {
                // Handle form submissions
                switch (CurrentAction)
                {
                    case "add":
                        HandleAddProduct();
                        break;
                    case "edit":
                        HandleEditProduct();
                        break;
                }
            }
            else
            {
                // Handle GET requests
                switch (CurrentAction)
                {
                    case "list":
                        LoadAllProducts();
                        break;
                    case "add":
                        CurrentProduct = new Product();
                        break;
                    case "edit":
                        HandleEditRequest();
                        break;
                    case "view":
                        HandleViewRequest();
                        break;
                    case "delete":
                        HandleDeleteRequest();
                        break;
                }
            }
        }

        /// <summary>
        /// Loads all products from the database
        /// </summary>
        private void LoadAllProducts()
        {
            try
            {
                AllProducts = Product.GetAllProducts();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error loading products: " + ex.Message;
                AllProducts = new List<Product>();
            }
        }

        /// <summary>
        /// Loads all categories for dropdown
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                AllCategories = Category.GetAllCategories();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error loading categories: " + ex.Message;
                AllCategories = new List<Category>();
            }
        }

        /// <summary>
        /// Handles the edit request - loads the product data
        /// </summary>
        private void HandleEditRequest()
        {
            try
            {
                int productID = 0;
                if (int.TryParse(Request.QueryString["id"], out productID))
                {
                    CurrentProduct = Product.GetProductByID(productID);
                    if (CurrentProduct == null)
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Product not found.";
                        LoadAllProducts();
                    }
                }
                else
                {
                    CurrentAction = "list";
                    ErrorMessage = "Invalid product ID.";
                    LoadAllProducts();
                }
            }
            catch (Exception ex)
            {
                CurrentAction = "list";
                ErrorMessage = "Error loading product: " + ex.Message;
                LoadAllProducts();
            }
        }

        /// <summary>
        /// Handles the view request - loads product data for display
        /// </summary>
        private void HandleViewRequest()
        {
            try
            {
                int productID = 0;
                if (int.TryParse(Request.QueryString["id"], out productID))
                {
                    CurrentProduct = Product.GetProductByID(productID);
                    if (CurrentProduct == null)
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Product not found.";
                        LoadAllProducts();
                    }
                }
                else
                {
                    CurrentAction = "list";
                    ErrorMessage = "Invalid product ID.";
                    LoadAllProducts();
                }
            }
            catch (Exception ex)
            {
                CurrentAction = "list";
                ErrorMessage = "Error loading product: " + ex.Message;
                LoadAllProducts();
            }
        }

        /// <summary>
        /// Handles adding a new product
        /// </summary>
        private void HandleAddProduct()
        {
            try
            {
                // Create a new product object from form data
                Product product = new Product
                {
                    ProductName = Request.Form["productName"],
                    CategoryID = Convert.ToInt32(Request.Form["categoryID"]),
                    UnitPrice = Convert.ToDecimal(Request.Form["unitPrice"]),
                    UnitsInStock = Convert.ToInt32(Request.Form["unitsInStock"]),
                    Description = Request.Form["description"],
                    ImagePath = Request.Form["imagePath"]
                };

                // Validate data
                if (string.IsNullOrEmpty(product.ProductName))
                {
                    ErrorMessage = "Product name is required.";
                    CurrentProduct = product;
                    return;
                }

                if (product.CategoryID <= 0)
                {
                    ErrorMessage = "Please select a category.";
                    CurrentProduct = product;
                    return;
                }

                // Insert the product
                if (product.Insert())
                {
                    // Redirect to the list page with success message
                    Response.Redirect("Products.aspx?message=Product added successfully.");
                }
                else
                {
                    ErrorMessage = "Failed to add product.";
                    CurrentProduct = product;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error adding product: " + ex.Message;
                CurrentProduct = new Product
                {
                    ProductName = Request.Form["productName"],
                    CategoryID = Convert.ToInt32(Request.Form["categoryID"]),
                    UnitPrice = Convert.ToDecimal(Request.Form["unitPrice"]),
                    UnitsInStock = Convert.ToInt32(Request.Form["unitsInStock"]),
                    Description = Request.Form["description"],
                    ImagePath = Request.Form["imagePath"]
                };
            }
        }

        /// <summary>
        /// Handles editing an existing product
        /// </summary>
        private void HandleEditProduct()
        {
            try
            {
                int productID = 0;
                if (int.TryParse(Request.QueryString["id"], out productID))
                {
                    // Get the existing product
                    Product product = Product.GetProductByID(productID);
                    if (product == null)
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Product not found.";
                        LoadAllProducts();
                        return;
                    }

                    // Update properties
                    product.ProductName = Request.Form["productName"];
                    product.CategoryID = Convert.ToInt32(Request.Form["categoryID"]);
                    product.UnitPrice = Convert.ToDecimal(Request.Form["unitPrice"]);
                    product.UnitsInStock = Convert.ToInt32(Request.Form["unitsInStock"]);
                    product.Description = Request.Form["description"];
                    product.ImagePath = Request.Form["imagePath"];

                    // Validate data
                    if (string.IsNullOrEmpty(product.ProductName))
                    {
                        ErrorMessage = "Product name is required.";
                        CurrentProduct = product;
                        return;
                    }

                    if (product.CategoryID <= 0)
                    {
                        ErrorMessage = "Please select a category.";
                        CurrentProduct = product;
                        return;
                    }

                    // Update the product
                    if (product.Update())
                    {
                        // Redirect to the list page with success message
                        Response.Redirect("Products.aspx?message=Product updated successfully.");
                    }
                    else
                    {
                        ErrorMessage = "Failed to update product.";
                        CurrentProduct = product;
                    }
                }
                else
                {
                    CurrentAction = "list";
                    ErrorMessage = "Invalid product ID.";
                    LoadAllProducts();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error updating product: " + ex.Message;
                CurrentProduct = new Product
                {
                    ProductID = Convert.ToInt32(Request.QueryString["id"]),
                    ProductName = Request.Form["productName"],
                    CategoryID = Convert.ToInt32(Request.Form["categoryID"]),
                    UnitPrice = Convert.ToDecimal(Request.Form["unitPrice"]),
                    UnitsInStock = Convert.ToInt32(Request.Form["unitsInStock"]),
                    Description = Request.Form["description"],
                    ImagePath = Request.Form["imagePath"]
                };
            }
        }

        /// <summary>
        /// Handles deleting a product
        /// </summary>
        private void HandleDeleteRequest()
        {
            try
            {
                int productID = 0;
                if (int.TryParse(Request.QueryString["id"], out productID))
                {
                    // Get the product
                    Product product = Product.GetProductByID(productID);
                    if (product == null)
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Product not found.";
                        LoadAllProducts();
                        return;
                    }

                    // Delete the product
                    if (product.Delete())
                    {
                        CurrentAction = "list";
                        SuccessMessage = "Product deleted successfully.";
                        LoadAllProducts();
                    }
                    else
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Failed to delete product.";
                        LoadAllProducts();
                    }
                }
                else
                {
                    CurrentAction = "list";
                    ErrorMessage = "Invalid product ID.";
                    LoadAllProducts();
                }
            }
            catch (Exception ex)
            {
                CurrentAction = "list";
                ErrorMessage = "Error deleting product: " + ex.Message;
                LoadAllProducts();
            }
        }
    }
} 