using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using ProductManagementSystem.Models;

namespace ProductManagementSystem
{
    public partial class Default : System.Web.UI.Page
    {
        public int CategoryCount { get; private set; }
        public int ProductCount { get; private set; }
        public int LowStockCount { get; private set; }
        public List<Product> RecentProducts { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
            }
        }

        /// <summary>
        /// Loads the dashboard data for display
        /// </summary>
        private void LoadDashboardData()
        {
            try
            {
                // Get category count
                string categoryQuery = "SELECT COUNT(*) FROM Categories";
                CategoryCount = Convert.ToInt32(Database.ExecuteScalar(categoryQuery));

                // Get product count
                string productQuery = "SELECT COUNT(*) FROM Products";
                ProductCount = Convert.ToInt32(Database.ExecuteScalar(productQuery));

                // Get low stock count
                string lowStockQuery = "SELECT COUNT(*) FROM Products WHERE UnitsInStock <= 10";
                LowStockCount = Convert.ToInt32(Database.ExecuteScalar(lowStockQuery));

                // Get recent products (5 most recent)
                LoadRecentProducts();
            }
            catch (Exception ex)
            {
                // Log the error (in a real application)
                // For now, just display an error message
                RecentProducts = new List<Product>();
                ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                    "showMessage('Error loading dashboard data: " + HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        /// <summary>
        /// Loads the 5 most recently added products
        /// </summary>
        private void LoadRecentProducts()
        {
            try
            {
                string query = @"
                    SELECT TOP 5 p.ProductID, p.ProductName, p.CategoryID, p.UnitPrice, p.UnitsInStock,
                           p.Description, p.ImagePath, p.CreatedDate, c.CategoryName
                    FROM Products p
                    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                    ORDER BY p.CreatedDate DESC";

                DataTable dataTable = Database.ExecuteQuery(query);
                RecentProducts = new List<Product>();

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

                    RecentProducts.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading recent products: " + ex.Message);
            }
        }
    }
} 