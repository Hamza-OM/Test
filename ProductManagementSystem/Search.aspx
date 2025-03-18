<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="ProductManagementSystem.Search" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Search - Product Management System</title>
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
                <li><a href="LowStock.aspx">Low Stock</a></li>
                <li><a href="Search.aspx" class="active">Search</a></li>
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
            </div>
            
            <h2 class="page-title">Search Products</h2>
            
            <div class="card">
                <div class="card-body">
                    <form method="get" action="Search.aspx">
                        <div class="row">
                            <div class="col">
                                <div class="form-group">
                                    <label for="q">Search Term:</label>
                                    <input type="text" id="q" name="q" class="form-control" value="<%= Server.HtmlEncode(SearchTerm) %>" placeholder="Enter product name or description..." required />
                                </div>
                            </div>
                            <div class="col">
                                <div class="form-group">
                                    <label for="category">Filter by Category:</label>
                                    <select id="category" name="category" class="form-control">
                                        <option value="0">All Categories</option>
                                        <% foreach (var category in AllCategories) { %>
                                        <option value="<%= category.CategoryID %>" <%= CategoryFilter == category.CategoryID ? "selected" : "" %>>
                                            <%= Server.HtmlEncode(category.CategoryName) %>
                                        </option>
                                        <% } %>
                                    </select>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row" style="margin-top: 15px;">
                            <div class="col">
                                <div class="form-group">
                                    <label for="minPrice">Min Price:</label>
                                    <input type="number" id="minPrice" name="minPrice" step="0.01" min="0" class="form-control" value="<%= MinPrice > 0 ? MinPrice.ToString() : "" %>" placeholder="Min price..." />
                                </div>
                            </div>
                            <div class="col">
                                <div class="form-group">
                                    <label for="maxPrice">Max Price:</label>
                                    <input type="number" id="maxPrice" name="maxPrice" step="0.01" min="0" class="form-control" value="<%= MaxPrice > 0 ? MaxPrice.ToString() : "" %>" placeholder="Max price..." />
                                </div>
                            </div>
                            <div class="col">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <button type="submit" class="btn form-control">Search</button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
            
            <% if (!string.IsNullOrEmpty(SearchTerm) || CategoryFilter > 0 || MinPrice > 0 || MaxPrice > 0) { %>
            <!-- Search Results -->
            <div style="margin-top: 20px;">
                <h3>Search Results</h3>
                
                <% if (SearchResults.Count > 0) { %>
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
                        <% foreach (var product in SearchResults) { %>
                        <tr>
                            <td><%= Server.HtmlEncode(product.ProductName) %></td>
                            <td><%= Server.HtmlEncode(product.CategoryName) %></td>
                            <td>$<%= product.UnitPrice.ToString("0.00") %></td>
                            <td>
                                <%= product.UnitsInStock %>
                                <% if (product.UnitsInStock <= 10) { %>
                                <span style="color: #e74c3c; font-weight: bold;" title="Low stock!">&nbsp;⚠</span>
                                <% } %>
                            </td>
                            <td class="actions">
                                <a href="Products.aspx?action=edit&id=<%= product.ProductID %>" class="btn btn-secondary">Edit</a>
                                <a href="Products.aspx?action=view&id=<%= product.ProductID %>" class="btn">View</a>
                            </td>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
                <% } else { %>
                <div class="alert alert-info">
                    No products found matching your search criteria.
                </div>
                <% } %>
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