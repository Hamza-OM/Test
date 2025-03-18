<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LowStock.aspx.cs" Inherits="ProductManagementSystem.LowStock" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Low Stock - Product Management System</title>
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
                <li><a href="default.aspx">Home</a></li>
                <li><a href="Products.aspx">Products</a></li>
                <li><a href="Categories.aspx">Categories</a></li>
                <li><a href="LowStock.aspx" class="active">Low Stock</a></li>
                <li><a href="Search.aspx">Search</a></li>
            </ul>
        </div>
    </nav>
    
    <main>
        <div class="container">
            <div id="messageContainer">
                <% if (!string.IsNullOrEmpty(ErrorMessage)) { %>
                <div class="alert alert-danger">
                    <%= Server.HtmlEncode(ErrorMessage) %>
                </div>
                <% } %>
                <% if (!string.IsNullOrEmpty(SuccessMessage)) { %>
                <div class="alert alert-success">
                    <%= Server.HtmlEncode(SuccessMessage) %>
                </div>
                <% } %>
            </div>
            
            <h2 class="page-title">Low Stock Products</h2>
            
            <div class="row" style="margin-bottom: 20px;">
                <div class="col">
                    <p>Products with stock levels at or below <%= StockThreshold %> units are shown below.</p>
                </div>
                <div class="col">
                    <form method="get" action="LowStock.aspx" class="search-box">
                        <input type="number" name="threshold" min="1" value="<%= StockThreshold %>" placeholder="Stock threshold..." />
                        <button type="submit">Update Threshold</button>
                    </form>
                </div>
            </div>
            
            <table class="data-table" id="lowStockTable">
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
                    <% foreach (var product in LowStockProducts) { %>
                    <tr class="product-row">
                        <td class="product-name"><%= Server.HtmlEncode(product.ProductName) %></td>
                        <td class="category-name"><%= Server.HtmlEncode(product.CategoryName) %></td>
                        <td>$<%= product.UnitPrice.ToString("0.00") %></td>
                        <td>
                            <div class="stock-update">
                                <input type="number" min="0" value="<%= product.UnitsInStock %>" id="stock-<%= product.ProductID %>" />
                                <button type="button" onclick="updateStock(<%= product.ProductID %>, $('#stock-<%= product.ProductID %>').val())">Update</button>
                            </div>
                        </td>
                        <td class="actions">
                            <a href="Products.aspx?action=edit&id=<%= product.ProductID %>" class="btn btn-secondary">Edit</a>
                            <a href="Products.aspx?action=view&id=<%= product.ProductID %>" class="btn">View</a>
                        </td>
                    </tr>
                    <% } %>
                    <% if (LowStockProducts.Count == 0) { %>
                    <tr>
                        <td colspan="5" class="empty-message">No products with stock levels at or below <%= StockThreshold %> units.</td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
            
            <% if (LowStockProducts.Count > 0) { %>
            <div style="margin-top: 20px;">
                <form method="post" action="LowStock.aspx">
                    <div class="form-group">
                        <label for="increaseAmount">Quick Restock - Add to all low stock products:</label>
                        <div style="display: flex; gap: 10px;">
                            <input type="number" id="increaseAmount" name="increaseAmount" min="1" value="10" class="form-control" style="max-width: 100px;" required />
                            <button type="submit" class="btn btn-success">Restock All</button>
                        </div>
                    </div>
                </form>
            </div>
            <% } %>
        </div>
    </main>
    
    <footer>
        <div class="container">
            <p>&copy; 2025 Product Management System. All rights reserved.</p>
        </div>
    </footer>
</body>
</html> 