using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using ProductManagementSystem.Models;

namespace ProductManagementSystem
{
    public partial class Categories : System.Web.UI.Page
    {
        public string CurrentAction { get; private set; }
        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }
        public List<Category> AllCategories { get; private set; }
        public Category CurrentCategory { get; private set; }
        public List<CategoryStatistics> CategoryProductCounts { get; private set; }

        // Helper class for category statistics
        public class CategoryStatistics
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public int ProductCount { get; set; }
            public decimal TotalValue { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the current action from the query string
            CurrentAction = Request.QueryString["action"] ?? "list";
            
            if (IsPostBack)
            {
                // Handle form submissions
                switch (CurrentAction)
                {
                    case "add":
                        HandleAddCategory();
                        break;
                    case "edit":
                        HandleEditCategory();
                        break;
                }
            }
            else
            {
                // Handle GET requests
                switch (CurrentAction)
                {
                    case "list":
                        LoadAllCategories();
                        LoadCategoryStatistics();
                        break;
                    case "add":
                        CurrentCategory = new Category();
                        break;
                    case "edit":
                        HandleEditRequest();
                        break;
                    case "delete":
                        HandleDeleteRequest();
                        break;
                }
            }
        }

        /// <summary>
        /// Loads all categories from the database
        /// </summary>
        private void LoadAllCategories()
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
        /// Loads category statistics (product counts and values)
        /// This is an additional operation beyond basic CRUD
        /// </summary>
        private void LoadCategoryStatistics()
        {
            try
            {
                string query = @"
                    SELECT c.CategoryID, c.CategoryName, 
                           COUNT(p.ProductID) AS ProductCount,
                           SUM(p.UnitPrice * p.UnitsInStock) AS TotalValue
                    FROM Categories c
                    LEFT JOIN Products p ON c.CategoryID = p.CategoryID
                    GROUP BY c.CategoryID, c.CategoryName
                    ORDER BY c.CategoryName";

                DataTable dataTable = Database.ExecuteQuery(query);
                CategoryProductCounts = new List<CategoryStatistics>();

                foreach (DataRow row in dataTable.Rows)
                {
                    CategoryStatistics stats = new CategoryStatistics
                    {
                        CategoryID = Convert.ToInt32(row["CategoryID"]),
                        CategoryName = row["CategoryName"].ToString(),
                        ProductCount = Convert.ToInt32(row["ProductCount"]),
                        TotalValue = row["TotalValue"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalValue"])
                    };

                    CategoryProductCounts.Add(stats);
                }
            }
            catch (Exception ex)
            {
                // Just log the error but don't show it - statistics are optional
                System.Diagnostics.Debug.WriteLine("Error loading category statistics: " + ex.Message);
                CategoryProductCounts = new List<CategoryStatistics>();
            }
        }

        /// <summary>
        /// Handles the edit request - loads the category data
        /// </summary>
        private void HandleEditRequest()
        {
            try
            {
                int categoryID = 0;
                if (int.TryParse(Request.QueryString["id"], out categoryID))
                {
                    CurrentCategory = Category.GetCategoryByID(categoryID);
                    if (CurrentCategory == null)
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Category not found.";
                        LoadAllCategories();
                        LoadCategoryStatistics();
                    }
                }
                else
                {
                    CurrentAction = "list";
                    ErrorMessage = "Invalid category ID.";
                    LoadAllCategories();
                    LoadCategoryStatistics();
                }
            }
            catch (Exception ex)
            {
                CurrentAction = "list";
                ErrorMessage = "Error loading category: " + ex.Message;
                LoadAllCategories();
                LoadCategoryStatistics();
            }
        }

        /// <summary>
        /// Handles adding a new category
        /// </summary>
        private void HandleAddCategory()
        {
            try
            {
                // Create a new category object from form data
                Category category = new Category
                {
                    CategoryName = Request.Form["categoryName"],
                    Description = Request.Form["description"]
                };

                // Validate data
                if (string.IsNullOrEmpty(category.CategoryName))
                {
                    ErrorMessage = "Category name is required.";
                    CurrentCategory = category;
                    return;
                }

                // Insert the category
                if (category.Insert())
                {
                    // Redirect to the list page with success message
                    Response.Redirect("Categories.aspx?message=Category added successfully.");
                }
                else
                {
                    ErrorMessage = "Failed to add category.";
                    CurrentCategory = category;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error adding category: " + ex.Message;
                CurrentCategory = new Category
                {
                    CategoryName = Request.Form["categoryName"],
                    Description = Request.Form["description"]
                };
            }
        }

        /// <summary>
        /// Handles editing an existing category
        /// </summary>
        private void HandleEditCategory()
        {
            try
            {
                int categoryID = 0;
                if (int.TryParse(Request.QueryString["id"], out categoryID))
                {
                    // Get the existing category
                    Category category = Category.GetCategoryByID(categoryID);
                    if (category == null)
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Category not found.";
                        LoadAllCategories();
                        LoadCategoryStatistics();
                        return;
                    }

                    // Update properties
                    category.CategoryName = Request.Form["categoryName"];
                    category.Description = Request.Form["description"];

                    // Validate data
                    if (string.IsNullOrEmpty(category.CategoryName))
                    {
                        ErrorMessage = "Category name is required.";
                        CurrentCategory = category;
                        return;
                    }

                    // Update the category
                    if (category.Update())
                    {
                        // Redirect to the list page with success message
                        Response.Redirect("Categories.aspx?message=Category updated successfully.");
                    }
                    else
                    {
                        ErrorMessage = "Failed to update category.";
                        CurrentCategory = category;
                    }
                }
                else
                {
                    CurrentAction = "list";
                    ErrorMessage = "Invalid category ID.";
                    LoadAllCategories();
                    LoadCategoryStatistics();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error updating category: " + ex.Message;
                CurrentCategory = new Category
                {
                    CategoryID = Convert.ToInt32(Request.QueryString["id"]),
                    CategoryName = Request.Form["categoryName"],
                    Description = Request.Form["description"]
                };
            }
        }

        /// <summary>
        /// Handles deleting a category
        /// </summary>
        private void HandleDeleteRequest()
        {
            try
            {
                int categoryID = 0;
                if (int.TryParse(Request.QueryString["id"], out categoryID))
                {
                    // Check if there are products in this category
                    string checkQuery = "SELECT COUNT(*) FROM Products WHERE CategoryID = @CategoryID";
                    int productCount = Convert.ToInt32(Database.ExecuteScalar(checkQuery, 
                        new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@CategoryID", categoryID) }));

                    if (productCount > 0)
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Cannot delete category: There are " + productCount + " products assigned to this category. Please reassign or delete these products first.";
                        LoadAllCategories();
                        LoadCategoryStatistics();
                        return;
                    }

                    // Get the category
                    Category category = Category.GetCategoryByID(categoryID);
                    if (category == null)
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Category not found.";
                        LoadAllCategories();
                        LoadCategoryStatistics();
                        return;
                    }

                    // Delete the category
                    if (category.Delete())
                    {
                        CurrentAction = "list";
                        SuccessMessage = "Category deleted successfully.";
                        LoadAllCategories();
                        LoadCategoryStatistics();
                    }
                    else
                    {
                        CurrentAction = "list";
                        ErrorMessage = "Failed to delete category.";
                        LoadAllCategories();
                        LoadCategoryStatistics();
                    }
                }
                else
                {
                    CurrentAction = "list";
                    ErrorMessage = "Invalid category ID.";
                    LoadAllCategories();
                    LoadCategoryStatistics();
                }
            }
            catch (Exception ex)
            {
                CurrentAction = "list";
                ErrorMessage = "Error deleting category: " + ex.Message;
                LoadAllCategories();
                LoadCategoryStatistics();
            }
        }
    }
} 