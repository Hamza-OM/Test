using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProductManagementSystem.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }

        // Get all products with category names
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryName, 
                           p.Price, p.StockQuantity, p.Description 
                           FROM Products p 
                           LEFT JOIN Categories c ON p.CategoryID = c.CategoryID";
            
            DataTable dataTable = Database.ExecuteQuery(query);
            
            foreach (DataRow row in dataTable.Rows)
            {
                products.Add(new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    Description = row["Description"].ToString()
                });
            }
            
            return products;
        }

        // Get a specific product by ID
        public static Product GetProductByID(int productID)
        {
            string query = @"SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryName, 
                           p.Price, p.StockQuantity, p.Description 
                           FROM Products p 
                           LEFT JOIN Categories c ON p.CategoryID = c.CategoryID 
                           WHERE p.ProductID = @ProductID";
                           
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", productID)
            };
            
            DataTable dataTable = Database.ExecuteQuery(query, parameters);
            
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    Description = row["Description"].ToString()
                };
            }
            
            return null;
        }

        // Get products by category ID
        public static List<Product> GetProductsByCategoryID(int categoryID)
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryName, 
                           p.Price, p.StockQuantity, p.Description 
                           FROM Products p 
                           LEFT JOIN Categories c ON p.CategoryID = c.CategoryID 
                           WHERE p.CategoryID = @CategoryID";
                           
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID", categoryID)
            };
            
            DataTable dataTable = Database.ExecuteQuery(query, parameters);
            
            foreach (DataRow row in dataTable.Rows)
            {
                products.Add(new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    Description = row["Description"].ToString()
                });
            }
            
            return products;
        }

        // Insert a new product
        public int Insert()
        {
            string query = @"INSERT INTO Products (ProductName, CategoryID, Price, StockQuantity, Description) 
                           VALUES (@ProductName, @CategoryID, @Price, @StockQuantity, @Description); 
                           SELECT SCOPE_IDENTITY()";
                           
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductName", ProductName),
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@Price", Price),
                new SqlParameter("@StockQuantity", StockQuantity),
                new SqlParameter("@Description", Description ?? (object)DBNull.Value)
            };
            
            object result = Database.ExecuteScalar(query, parameters);
            
            if (result != null && result != DBNull.Value)
            {
                ProductID = Convert.ToInt32(result);
                return ProductID;
            }
            
            return 0;
        }

        // Update an existing product
        public bool Update()
        {
            string query = @"UPDATE Products 
                           SET ProductName = @ProductName, 
                               CategoryID = @CategoryID, 
                               Price = @Price, 
                               StockQuantity = @StockQuantity, 
                               Description = @Description 
                           WHERE ProductID = @ProductID";
                           
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", ProductID),
                new SqlParameter("@ProductName", ProductName),
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@Price", Price),
                new SqlParameter("@StockQuantity", StockQuantity),
                new SqlParameter("@Description", Description ?? (object)DBNull.Value)
            };
            
            int rowsAffected = Database.ExecuteNonQuery(query, parameters);
            
            return rowsAffected > 0;
        }

        // Delete a product
        public bool Delete()
        {
            string query = "DELETE FROM Products WHERE ProductID = @ProductID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", ProductID)
            };
            
            int rowsAffected = Database.ExecuteNonQuery(query, parameters);
            
            return rowsAffected > 0;
        }
        
        // Search products by name
        public static List<Product> SearchProducts(string searchTerm)
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryName, 
                           p.Price, p.StockQuantity, p.Description 
                           FROM Products p 
                           LEFT JOIN Categories c ON p.CategoryID = c.CategoryID 
                           WHERE p.ProductName LIKE @SearchTerm OR p.Description LIKE @SearchTerm";
                           
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchTerm", "%" + searchTerm + "%")
            };
            
            DataTable dataTable = Database.ExecuteQuery(query, parameters);
            
            foreach (DataRow row in dataTable.Rows)
            {
                products.Add(new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    Description = row["Description"].ToString()
                });
            }
            
            return products;
        }
        
        // Get low stock products (less than specified quantity)
        public static List<Product> GetLowStockProducts(int threshold = 10)
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryName, 
                           p.Price, p.StockQuantity, p.Description 
                           FROM Products p 
                           LEFT JOIN Categories c ON p.CategoryID = c.CategoryID 
                           WHERE p.StockQuantity < @Threshold";
                           
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Threshold", threshold)
            };
            
            DataTable dataTable = Database.ExecuteQuery(query, parameters);
            
            foreach (DataRow row in dataTable.Rows)
            {
                products.Add(new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    Description = row["Description"].ToString()
                });
            }
            
            return products;
        }
    }
} 