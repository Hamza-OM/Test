using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProductManagementSystem.Models
{
    /// <summary>
    /// Category model class
    /// </summary>
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Category()
        {
            CategoryID = 0;
            CategoryName = string.Empty;
            Description = string.Empty;
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public Category(int categoryID, string categoryName, string description, DateTime createdDate)
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
            Description = description;
            CreatedDate = createdDate;
        }

        #region Database Operations

        /// <summary>
        /// Gets all categories from the database
        /// </summary>
        public static List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            string query = "SELECT CategoryID, CategoryName, Description, CreatedDate FROM Categories ORDER BY CategoryName";
            
            try
            {
                DataTable dataTable = Database.ExecuteQuery(query);
                
                foreach (DataRow row in dataTable.Rows)
                {
                    Category category = new Category
                    {
                        CategoryID = Convert.ToInt32(row["CategoryID"]),
                        CategoryName = row["CategoryName"].ToString(),
                        Description = row["Description"].ToString(),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                    };
                    
                    categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting categories: " + ex.Message);
            }
            
            return categories;
        }

        /// <summary>
        /// Gets a category by ID
        /// </summary>
        public static Category GetCategoryByID(int categoryID)
        {
            Category category = null;
            string query = "SELECT CategoryID, CategoryName, Description, CreatedDate FROM Categories WHERE CategoryID = @CategoryID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID", categoryID)
            };
            
            try
            {
                DataTable dataTable = Database.ExecuteQuery(query, parameters);
                
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    category = new Category
                    {
                        CategoryID = Convert.ToInt32(row["CategoryID"]),
                        CategoryName = row["CategoryName"].ToString(),
                        Description = row["Description"].ToString(),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting category: " + ex.Message);
            }
            
            return category;
        }

        /// <summary>
        /// Inserts a new category
        /// </summary>
        public bool Insert()
        {
            string query = "INSERT INTO Categories (CategoryName, Description) VALUES (@CategoryName, @Description); SELECT SCOPE_IDENTITY();";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryName", CategoryName),
                new SqlParameter("@Description", Description)
            };
            
            try
            {
                object result = Database.ExecuteScalar(query, parameters);
                if (result != null && result != DBNull.Value)
                {
                    CategoryID = Convert.ToInt32(result);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting category: " + ex.Message);
            }
            
            return false;
        }

        /// <summary>
        /// Updates an existing category
        /// </summary>
        public bool Update()
        {
            string query = "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description WHERE CategoryID = @CategoryID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@CategoryName", CategoryName),
                new SqlParameter("@Description", Description)
            };
            
            try
            {
                int rowsAffected = Database.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating category: " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes a category
        /// </summary>
        public bool Delete()
        {
            string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID", CategoryID)
            };
            
            try
            {
                int rowsAffected = Database.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category: " + ex.Message);
            }
        }

        #endregion
    }
} 