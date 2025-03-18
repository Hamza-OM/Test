using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProductManagementSystem.Models
{
    /// <summary>
    /// Product model class
    /// </summary>
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Navigation property
        public string CategoryName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Product()
        {
            ProductID = 0;
            ProductName = string.Empty;
            CategoryID = 0;
            UnitPrice = 0;
            UnitsInStock = 0;
            Description = string.Empty;
            ImagePath = string.Empty;
            CreatedDate = DateTime.Now;
            CategoryName = string.Empty;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public Product(int productID, string productName, int categoryID, decimal unitPrice, 
                      int unitsInStock, string description, string imagePath, DateTime createdDate)
        {
            ProductID = productID;
            ProductName = productName;
            CategoryID = categoryID;
            UnitPrice = unitPrice;
            UnitsInStock = unitsInStock;
            Description = description;
            ImagePath = imagePath;
            CreatedDate = createdDate;
        }

        #region Database Operations

        /// <summary>
        /// Gets all products from the database with category information
        /// </summary>
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            string query = @"
                SELECT p.ProductID, p.ProductName, p.CategoryID, p.UnitPrice, p.UnitsInStock,
                       p.Description, p.ImagePath, p.CreatedDate, c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                ORDER BY p.ProductName";
            
            try
            {
                DataTable dataTable = Database.ExecuteQuery(query);
                
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
                    
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting products: " + ex.Message);
            }
            
            return products;
        }

        /// <summary>
        /// Gets products by category ID
        /// </summary>
        public static List<Product> GetProductsByCategoryID(int categoryID)
        {
            List<Product> products = new List<Product>();
            string query = @"
                SELECT p.ProductID, p.ProductName, p.CategoryID, p.UnitPrice, p.UnitsInStock,
                       p.Description, p.ImagePath, p.CreatedDate, c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.CategoryID = @CategoryID
                ORDER BY p.ProductName";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID", categoryID)
            };
            
            try
            {
                DataTable dataTable = Database.ExecuteQuery(query, parameters);
                
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
                    
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting products by category: " + ex.Message);
            }
            
            return products;
        }

        /// <summary>
        /// Gets a product by ID
        /// </summary>
        public static Product GetProductByID(int productID)
        {
            Product product = null;
            string query = @"
                SELECT p.ProductID, p.ProductName, p.CategoryID, p.UnitPrice, p.UnitsInStock,
                       p.Description, p.ImagePath, p.CreatedDate, c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.ProductID = @ProductID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", productID)
            };
            
            try
            {
                DataTable dataTable = Database.ExecuteQuery(query, parameters);
                
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    product = new Product
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
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting product: " + ex.Message);
            }
            
            return product;
        }

        /// <summary>
        /// Inserts a new product
        /// </summary>
        public bool Insert()
        {
            string query = @"
                INSERT INTO Products (ProductName, CategoryID, UnitPrice, UnitsInStock, Description, ImagePath)
                VALUES (@ProductName, @CategoryID, @UnitPrice, @UnitsInStock, @Description, @ImagePath);
                SELECT SCOPE_IDENTITY();";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductName", ProductName),
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@UnitPrice", UnitPrice),
                new SqlParameter("@UnitsInStock", UnitsInStock),
                new SqlParameter("@Description", Description),
                new SqlParameter("@ImagePath", string.IsNullOrEmpty(ImagePath) ? DBNull.Value : (object)ImagePath)
            };
            
            try
            {
                object result = Database.ExecuteScalar(query, parameters);
                if (result != null && result != DBNull.Value)
                {
                    ProductID = Convert.ToInt32(result);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting product: " + ex.Message);
            }
            
            return false;
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        public bool Update()
        {
            string query = @"
                UPDATE Products
                SET ProductName = @ProductName,
                    CategoryID = @CategoryID,
                    UnitPrice = @UnitPrice,
                    UnitsInStock = @UnitsInStock,
                    Description = @Description,
                    ImagePath = @ImagePath
                WHERE ProductID = @ProductID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", ProductID),
                new SqlParameter("@ProductName", ProductName),
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@UnitPrice", UnitPrice),
                new SqlParameter("@UnitsInStock", UnitsInStock),
                new SqlParameter("@Description", Description),
                new SqlParameter("@ImagePath", string.IsNullOrEmpty(ImagePath) ? DBNull.Value : (object)ImagePath)
            };
            
            try
            {
                int rowsAffected = Database.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product: " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        public bool Delete()
        {
            string query = "DELETE FROM Products WHERE ProductID = @ProductID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", ProductID)
            };
            
            try
            {
                int rowsAffected = Database.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting product: " + ex.Message);
            }
        }

        /// <summary>
        /// Searches products by name or description (additional operation 1)
        /// </summary>
        public static List<Product> SearchProducts(string searchTerm)
        {
            List<Product> products = new List<Product>();
            string query = @"
                SELECT p.ProductID, p.ProductName, p.CategoryID, p.UnitPrice, p.UnitsInStock,
                       p.Description, p.ImagePath, p.CreatedDate, c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.ProductName LIKE @SearchTerm OR p.Description LIKE @SearchTerm
                ORDER BY p.ProductName";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchTerm", "%" + searchTerm + "%")
            };
            
            try
            {
                DataTable dataTable = Database.ExecuteQuery(query, parameters);
                
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
                    
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching products: " + ex.Message);
            }
            
            return products;
        }

        /// <summary>
        /// Gets products that are low in stock (additional operation 2)
        /// </summary>
        public static List<Product> GetLowStockProducts(int threshold = 10)
        {
            List<Product> products = new List<Product>();
            string query = @"
                SELECT p.ProductID, p.ProductName, p.CategoryID, p.UnitPrice, p.UnitsInStock,
                       p.Description, p.ImagePath, p.CreatedDate, c.CategoryName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE p.UnitsInStock <= @Threshold
                ORDER BY p.UnitsInStock, p.ProductName";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Threshold", threshold)
            };
            
            try
            {
                DataTable dataTable = Database.ExecuteQuery(query, parameters);
                
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
                    
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting low stock products: " + ex.Message);
            }
            
            return products;
        }

        #endregion
    }
} 