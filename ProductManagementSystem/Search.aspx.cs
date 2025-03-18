using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using ProductManagementSystem.Models;

namespace ProductManagementSystem
{
    public partial class Search : System.Web.UI.Page
    {
        public string ErrorMessage { get; private set; }
        public string SearchTerm { get; private set; }
        public int CategoryFilter { get; private set; }
        public decimal MinPrice { get; private set; }
        public decimal MaxPrice { get; private set; }
        public List<Category> AllCategories { get; private set; }
        public List<Product> SearchResults { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load categories for the filter dropdown
            LoadCategories();

            // Get search parameters from query string
            SearchTerm = Request.QueryString["q"] ?? string.Empty;
            
            int categoryID = 0;
            if (int.TryParse(Request.QueryString["category"], out categoryID))
            {
                CategoryFilter = categoryID;
            }
            
            decimal minPrice = 0;
            if (decimal.TryParse(Request.QueryString["minPrice"], out minPrice))
            {
                MinPrice = minPrice;
            }
            
            decimal maxPrice = 0;
            if (decimal.TryParse(Request.QueryString["maxPrice"], out maxPrice))
            {
                MaxPrice = maxPrice;
            }

            // Perform search if we have criteria
            if (!string.IsNullOrEmpty(SearchTerm) || CategoryFilter > 0 || MinPrice > 0 || MaxPrice > 0)
            {
                PerformSearch();
            }
            else
            {
                SearchResults = new List<Product>();
            }
        }

        /// <summary>
        /// Loads all categories for the filter dropdown
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
        /// Performs the advanced product search with filtering
        /// This is an additional operation beyond basic CRUD
        /// </summary>
        private void PerformSearch()
        {
            try
            {
                // Start with base query
                string query = @"
                    SELECT p.ProductID, p.ProductName, p.CategoryID, p.UnitPrice, p.UnitsInStock,
                           p.Description, p.ImagePath, p.CreatedDate, c.CategoryName
                    FROM Products p
                    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                    WHERE 1=1";
                
                // Create parameter list
                List<SqlParameter> parameters = new List<SqlParameter>();
                
                // Add search term filter if provided
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    query += " AND (p.ProductName LIKE @SearchTerm OR p.Description LIKE @SearchTerm)";
                    parameters.Add(new SqlParameter("@SearchTerm", "%" + SearchTerm + "%"));
                }
                
                // Add category filter if selected
                if (CategoryFilter > 0)
                {
                    query += " AND p.CategoryID = @CategoryID";
                    parameters.Add(new SqlParameter("@CategoryID", CategoryFilter));
                }
                
                // Add price range filters if provided
                if (MinPrice > 0)
                {
                    query += " AND p.UnitPrice >= @MinPrice";
                    parameters.Add(new SqlParameter("@MinPrice", MinPrice));
                }
                
                if (MaxPrice > 0)
                {
                    query += " AND p.UnitPrice <= @MaxPrice";
                    parameters.Add(new SqlParameter("@MaxPrice", MaxPrice));
                }
                
                // Add order by clause
                query += " ORDER BY p.ProductName";
                
                // Execute the query
                DataTable dataTable = Database.ExecuteQuery(query, parameters.ToArray());
                SearchResults = new List<Product>();
                
                // Process results
                foreach (DataRow row in dataTable.Rows)
                {
                    Product product = new Product
                    {
                        ProductID = Convert.ToInt32(row["ProductID"]),
                        ProductName = row["ProductName"].ToString(),
                        CategoryID = Convert.ToInt32(row["CategoryID"]),
                        UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                        UnitsInStock = Convert.ToInt32(row["UnitsInStock"]),
                        Description = row["Description"].ToString(),
                        ImagePath = row["ImagePath"] == DBNull.Value ? string.Empty : row["ImagePath"].ToString(),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                        CategoryName = row["CategoryName"].ToString()
                    };
                    
                    SearchResults.Add(product);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error performing search: " + ex.Message;
                SearchResults = new List<Product>();
            }
        }
    }
} 