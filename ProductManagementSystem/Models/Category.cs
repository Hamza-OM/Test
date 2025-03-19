using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProductManagementSystem.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Get all categories
        public static List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            string query = "SELECT CategoryID, CategoryName, Description FROM Categories";
            
            DataTable dataTable = Database.ExecuteQuery(query);
            
            foreach (DataRow row in dataTable.Rows)
            {
                categories.Add(new Category
                {
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Description = row["Description"].ToString()
                });
            }
            
            return categories;
        }

        // Get a specific category by ID
        public static Category GetCategoryByID(int categoryID)
        {
            string query = "SELECT CategoryID, CategoryName, Description FROM Categories WHERE CategoryID = @CategoryID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID", categoryID)
            };
            
            DataTable dataTable = Database.ExecuteQuery(query, parameters);
            
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Category
                {
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Description = row["Description"].ToString()
                };
            }
            
            return null;
        }

        // Insert a new category
        public int Insert()
        {
            string query = "INSERT INTO Categories (CategoryName, Description) VALUES (@CategoryName, @Description); SELECT SCOPE_IDENTITY()";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryName", CategoryName),
                new SqlParameter("@Description", Description ?? (object)DBNull.Value)
            };
            
            object result = Database.ExecuteScalar(query, parameters);
            
            if (result != null && result != DBNull.Value)
            {
                CategoryID = Convert.ToInt32(result);
                return CategoryID;
            }
            
            return 0;
        }

        // Update an existing category
        public bool Update()
        {
            string query = "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description WHERE CategoryID = @CategoryID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@CategoryName", CategoryName),
                new SqlParameter("@Description", Description ?? (object)DBNull.Value)
            };
            
            int rowsAffected = Database.ExecuteNonQuery(query, parameters);
            
            return rowsAffected > 0;
        }

        // Delete a category
        public bool Delete()
        {
            string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID", CategoryID)
            };
            
            int rowsAffected = Database.ExecuteNonQuery(query, parameters);
            
            return rowsAffected > 0;
        }
    }
} 