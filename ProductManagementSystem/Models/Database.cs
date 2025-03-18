using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace ProductManagementSystem.Models
{
    /// <summary>
    /// Database helper class for managing SQL Server connections and operations
    /// </summary>
    public class Database
    {
        // Connection string - in a real application, this would come from Web.config
        private static string connectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=ProductManagementDB;Integrated Security=True";

        /// <summary>
        /// Creates and returns a new SqlConnection object
        /// </summary>
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating database connection: " + ex.Message);
            }
        }

        /// <summary>
        /// Executes a SQL query and returns a DataTable with the results
        /// </summary>
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    DataTable dataTable = new DataTable();
                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error executing query: " + ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                    return dataTable;
                }
            }
        }

        /// <summary>
        /// Executes a non-query SQL command (INSERT, UPDATE, DELETE) and returns the number of rows affected
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    int rowsAffected = 0;
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error executing non-query: " + ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                    return rowsAffected;
                }
            }
        }

        /// <summary>
        /// Executes a SQL command and returns a single scalar value
        /// </summary>
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    object result = null;
                    try
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error executing scalar: " + ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                    return result;
                }
            }
        }
    }
} 