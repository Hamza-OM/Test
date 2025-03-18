using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using ProductManagementSystem.Models;
using System.Web.Script.Serialization;

namespace ProductManagementSystem
{
    /// <summary>
    /// This page handles AJAX requests to update product stock levels
    /// </summary>
    public partial class UpdateStock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Create a response object
            var response = new
            {
                success = false,
                message = string.Empty
            };

            try
            {
                // Check if this is a POST request
                if (Request.HttpMethod == "POST")
                {
                    // Get the product ID and new stock level from the request
                    int productID = 0;
                    int unitsInStock = 0;

                    if (int.TryParse(Request.Form["productID"], out productID) && 
                        int.TryParse(Request.Form["unitsInStock"], out unitsInStock) && 
                        productID > 0 && 
                        unitsInStock >= 0)
                    {
                        // Update the product's stock level in the database
                        string query = "UPDATE Products SET UnitsInStock = @UnitsInStock WHERE ProductID = @ProductID";
                        
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@ProductID", productID),
                            new SqlParameter("@UnitsInStock", unitsInStock)
                        };
                        
                        int rowsAffected = Database.ExecuteNonQuery(query, parameters);
                        
                        if (rowsAffected > 0)
                        {
                            // Success
                            response = new
                            {
                                success = true,
                                message = "Stock updated successfully"
                            };
                        }
                        else
                        {
                            // No rows affected - product not found
                            response = new
                            {
                                success = false,
                                message = "Product not found"
                            };
                        }
                    }
                    else
                    {
                        // Invalid parameters
                        response = new
                        {
                            success = false,
                            message = "Invalid parameters"
                        };
                    }
                }
                else
                {
                    // Not a POST request
                    response = new
                    {
                        success = false,
                        message = "Invalid request method"
                    };
                }
            }
            catch (Exception ex)
            {
                // Exception occurred
                response = new
                {
                    success = false,
                    message = "Error: " + ex.Message
                };
            }

            // Serialize the response to JSON and output it
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Response.Write(serializer.Serialize(response));
            Response.End();
        }
    }
} 