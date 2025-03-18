using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using ProductManagementSystem.Models;

namespace ProductManagementSystem
{
    public partial class LowStock : System.Web.UI.Page
    {
        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }
        public List<Product> LowStockProducts { get; private set; }
        public int StockThreshold { get; private set; } = 10; // Default threshold

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get threshold from query string if provided
            if (!string.IsNullOrEmpty(Request.QueryString["threshold"]))
            {
                int threshold;
                if (int.TryParse(Request.QueryString["threshold"], out threshold) && threshold > 0)
                {
                    StockThreshold = threshold;
                }
            }

            if (IsPostBack)
            {
                // Handle restock all functionality
                HandleRestockAll();
            }

            // Load low stock products
            LoadLowStockProducts();
        }

        /// <summary>
        /// Loads products with stock levels below the threshold
        /// </summary>
        private void LoadLowStockProducts()
        {
            try
            {
                LowStockProducts = Product.GetLowStockProducts(StockThreshold);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error loading low stock products: " + ex.Message;
                LowStockProducts = new List<Product>();
            }
        }

        /// <summary>
        /// Handles the "Restock All" functionality
        /// This is an additional operation beyond basic CRUD
        /// </summary>
        private void HandleRestockAll()
        {
            try
            {
                // Get the amount to increase stock by
                int increaseAmount = 0;
                if (int.TryParse(Request.Form["increaseAmount"], out increaseAmount) && increaseAmount > 0)
                {
                    // Update all products with low stock
                    string updateQuery = @"
                        UPDATE Products
                        SET UnitsInStock = UnitsInStock + @IncreaseAmount
                        WHERE UnitsInStock <= @Threshold";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@IncreaseAmount", increaseAmount),
                        new SqlParameter("@Threshold", StockThreshold)
                    };

                    int rowsAffected = Database.ExecuteNonQuery(updateQuery, parameters);
                    
                    if (rowsAffected > 0)
                    {
                        SuccessMessage = rowsAffected + " products have been restocked with " + increaseAmount + " additional units.";
                    }
                    else
                    {
                        ErrorMessage = "No products were updated. They may have been restocked by another user.";
                    }
                }
                else
                {
                    ErrorMessage = "Please enter a valid positive number for restocking.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error restocking products: " + ex.Message;
            }
        }
    }
} 