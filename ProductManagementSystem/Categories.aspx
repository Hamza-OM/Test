<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="ProductManagementSystem.Categories" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Categories - Product Management System</title>
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
                <li><a href="Categories.aspx" class="active">Categories</a></li>
                <li><a href="LowStock.aspx">Low Stock</a></li>
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
            
            <% if (CurrentAction == "list") { %>
            <!-- Category List View -->
            <h2 class="page-title">Categories</h2>
            
            <div class="row" style="margin-bottom: 20px;">
                <div class="col">
                    <a href="Categories.aspx?action=add" class="btn btn-success">Add New Category</a>
                </div>
                <div class="col">
                    <div class="search-box">
                        <input type="text" id="searchInput" placeholder="Search categories..." />
                        <button type="button" onclick="$('#searchInput').trigger('keyup');">Search</button>
                    </div>
                </div>
            </div>
            
            <table class="data-table" id="categoryTable">
                <thead>
                    <tr>
                        <th>Category Name</th>
                        <th>Description</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (var category in AllCategories) { %>
                    <tr class="category-row">
                        <td class="category-name"><%= Server.HtmlEncode(category.CategoryName) %></td>
                        <td class="description"><%= Server.HtmlEncode(category.Description) %></td>
                        <td class="actions">
                            <a href="Categories.aspx?action=edit&id=<%= category.CategoryID %>" class="btn btn-secondary">Edit</a>
                            <a href="Products.aspx?category=<%= category.CategoryID %>" class="btn">View Products</a>
                            <a href="Categories.aspx?action=delete&id=<%= category.CategoryID %>" class="btn btn-danger delete-btn">Delete</a>
                        </td>
                    </tr>
                    <% } %>
                    <% if (AllCategories.Count == 0) { %>
                    <tr>
                        <td colspan="3" class="empty-message">No categories found in the system.</td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
            <% } else if (CurrentAction == "add" || CurrentAction == "edit") { %>
            <!-- Add/Edit Category Form -->
            <h2 class="page-title"><%= CurrentAction == "add" ? "Add New Category" : "Edit Category" %></h2>
            
            <div class="form-container">
                <form id="categoryForm" method="post" action="Categories.aspx?action=<%= CurrentAction %><%= CurrentAction == "edit" ? "&id=" + CurrentCategory.CategoryID : "" %>">
                    <div class="form-group">
                        <label for="categoryName">Category Name:</label>
                        <input type="text" id="categoryName" name="categoryName" class="form-control" value="<%= Server.HtmlEncode(CurrentCategory.CategoryName) %>" required />
                    </div>
                    
                    <div class="form-group">
                        <label for="description">Description:</label>
                        <textarea id="description" name="description" class="form-control"><%= Server.HtmlEncode(CurrentCategory.Description) %></textarea>
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <button type="submit" class="btn btn-success">Save Category</button>
                        <a href="Categories.aspx" class="btn btn-secondary">Cancel</a>
                    </div>
                </form>
            </div>
            <% } %>
            
            <% if (CurrentAction == "list" && CategoryProductCounts.Count > 0) { %>
            <!-- Category Statistics -->
            <div class="card" style="margin-top: 20px;">
                <div class="card-header">
                    <h3>Category Statistics</h3>
                </div>
                <div class="card-body">
                    <table class="data-table">
                        <thead>
                            <tr>
                                <th>Category</th>
                                <th>Number of Products</th>
                                <th>Total Value</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var item in CategoryProductCounts) { %>
                            <tr>
                                <td><%= Server.HtmlEncode(item.CategoryName) %></td>
                                <td><%= item.ProductCount %></td>
                                <td>$<%= item.TotalValue.ToString("0.00") %></td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
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