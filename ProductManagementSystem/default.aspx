<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProductManagementSystem.Default" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home - Product Management System</title>
    <link rel="stylesheet" href="Content/Styles.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="Scripts/main.js"></script>
</head>
<body>
    <header>
        <div class="container">
            <h1>Product Management System</h1>
        </div>
    </header>
    
    <nav>
        <div class="container">
            <ul>
                <li><a href="default.aspx" class="active">Home</a></li>
                <li><a href="Products.aspx">Products</a></li>
                <li><a href="Categories.aspx">Categories</a></li>
                <li><a href="LowStock.aspx">Low Stock</a></li>
                <li><a href="Search.aspx">Search</a></li>
            </ul>
        </div>
    </nav>
    
    <main>
        <div class="container">
            <h2 class="page-title">Welcome to Product Management System</h2>
            
            <div id="messageContainer">
                <% if (!string.IsNullOrEmpty(Request.QueryString["message"])) { %>
                <div class="alert alert-success">
                    <%= Server.HtmlEncode(Request.QueryString["message"]) %>
                </div>
                <% } %>
            </div>
            
            <div class="row">
                <div class="col">
                    <div class="card">
                        <div class="card-header">
                            <h3>System Summary</h3>
                        </div>
                        <div class="card-body">
                            <p>Total Categories: <strong><%= CategoryCount %></strong></p>
                            <p>Total Products: <strong><%= ProductCount %></strong></p>
                            <p>Low Stock Products: <strong><%= LowStockCount %></strong></p>
                        </div>
                    </div>
                </div>
                
                <div class="col">
                    <div class="card">
                        <div class="card-header">
                            <h3>Quick Actions</h3>
                        </div>
                        <div class="card-body">
                            <p><a href="Products.aspx?action=add" class="btn btn-success">Add New Product</a></p>
                            <p><a href="Categories.aspx?action=add" class="btn">Add New Category</a></p>
                            <p><a href="LowStock.aspx" class="btn btn-secondary">View Low Stock Items</a></p>
                            <p><a href="Search.aspx" class="btn">Search Products</a></p>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="card" style="margin-top: 20px;">
                <div class="card-header">
                    <h3>Recent Products</h3>
                </div>
                <div class="card-body">
                    <table class="data-table">
                        <thead>
                            <tr>
                                <th>Product Name</th>
                                <th>Category</th>
                                <th>Price</th>
                                <th>Stock</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var product in RecentProducts) { %>
                            <tr>
                                <td><%= Server.HtmlEncode(product.ProductName) %></td>
                                <td><%= Server.HtmlEncode(product.CategoryName) %></td>
                                <td>$<%= product.UnitPrice.ToString("0.00") %></td>
                                <td><%= product.UnitsInStock %></td>
                                <td class="actions">
                                    <a href="Products.aspx?action=edit&id=<%= product.ProductID %>" class="btn btn-secondary">Edit</a>
                                    <a href="Products.aspx?action=view&id=<%= product.ProductID %>" class="btn">View</a>
                                </td>
                            </tr>
                            <% } %>
                            <% if (RecentProducts.Count == 0) { %>
                            <tr>
                                <td colspan="5" class="empty-message">No products found in the system.</td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </main>
    
    <footer>
        <div class="container">
            <p>&copy; 2025 Product Management System. All rights reserved.</p>
        </div>
    </footer>
</body>
</html> 