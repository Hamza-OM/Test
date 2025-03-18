<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="ProductManagementSystem.Products" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Products - Product Management System</title>
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
                <li><a href="Products.aspx" class="active">Products</a></li>
                <li><a href="Categories.aspx">Categories</a></li>
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
            <!-- Product List View -->
            <h2 class="page-title">Products</h2>
            
            <div class="row" style="margin-bottom: 20px;">
                <div class="col">
                    <a href="Products.aspx?action=add" class="btn btn-success">Add New Product</a>
                </div>
                <div class="col">
                    <div class="search-box">
                        <input type="text" id="searchInput" placeholder="Search products..." />
                        <button type="button" onclick="$('#searchInput').trigger('keyup');">Search</button>
                    </div>
                </div>
            </div>
            
            <table class="data-table" id="productTable">
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
                    <% foreach (var product in AllProducts) { %>
                    <tr class="product-row">
                        <td class="product-name"><%= Server.HtmlEncode(product.ProductName) %></td>
                        <td class="category-name"><%= Server.HtmlEncode(product.CategoryName) %></td>
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
                            <a href="Products.aspx?action=delete&id=<%= product.ProductID %>" class="btn btn-danger delete-btn">Delete</a>
                        </td>
                    </tr>
                    <% } %>
                    <% if (AllProducts.Count == 0) { %>
                    <tr>
                        <td colspan="5" class="empty-message">No products found in the system.</td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
            <% } else if (CurrentAction == "add" || CurrentAction == "edit") { %>
            <!-- Add/Edit Product Form -->
            <h2 class="page-title"><%= CurrentAction == "add" ? "Add New Product" : "Edit Product" %></h2>
            
            <div class="form-container">
                <form id="productForm" method="post" action="Products.aspx?action=<%= CurrentAction %><%= CurrentAction == "edit" ? "&id=" + CurrentProduct.ProductID : "" %>">
                    <div class="form-group">
                        <label for="productName">Product Name:</label>
                        <input type="text" id="productName" name="productName" class="form-control" value="<%= Server.HtmlEncode(CurrentProduct.ProductName) %>" required />
                    </div>
                    
                    <div class="form-group">
                        <label for="categoryID">Category:</label>
                        <select id="categoryID" name="categoryID" class="form-control" required>
                            <option value="0">-- Select Category --</option>
                            <% foreach (var category in AllCategories) { %>
                            <option value="<%= category.CategoryID %>" <%= CurrentProduct.CategoryID == category.CategoryID ? "selected" : "" %>>
                                <%= Server.HtmlEncode(category.CategoryName) %>
                            </option>
                            <% } %>
                        </select>
                    </div>
                    
                    <div class="form-group">
                        <label for="unitPrice">Unit Price:</label>
                        <input type="number" id="unitPrice" name="unitPrice" step="0.01" min="0" class="form-control" value="<%= CurrentProduct.UnitPrice %>" required />
                    </div>
                    
                    <div class="form-group">
                        <label for="unitsInStock">Units In Stock:</label>
                        <input type="number" id="unitsInStock" name="unitsInStock" min="0" class="form-control" value="<%= CurrentProduct.UnitsInStock %>" required />
                    </div>
                    
                    <div class="form-group">
                        <label for="description">Description:</label>
                        <textarea id="description" name="description" class="form-control"><%= Server.HtmlEncode(CurrentProduct.Description) %></textarea>
                    </div>
                    
                    <div class="form-group">
                        <label for="imagePath">Image URL (optional):</label>
                        <input type="text" id="imagePath" name="imagePath" class="form-control" value="<%= Server.HtmlEncode(CurrentProduct.ImagePath) %>" />
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <button type="submit" class="btn btn-success">Save Product</button>
                        <a href="Products.aspx" class="btn btn-secondary">Cancel</a>
                    </div>
                </form>
            </div>
            <% } else if (CurrentAction == "view") { %>
            <!-- View Product Details -->
            <h2 class="page-title">Product Details</h2>
            
            <div class="row">
                <div class="col">
                    <div class="card">
                        <div class="card-header">
                            <h3><%= Server.HtmlEncode(CurrentProduct.ProductName) %></h3>
                        </div>
                        <div class="card-body">
                            <% if (!string.IsNullOrEmpty(CurrentProduct.ImagePath)) { %>
                            <div style="text-align: center; margin-bottom: 20px;">
                                <img src="<%= Server.HtmlEncode(CurrentProduct.ImagePath) %>" alt="<%= Server.HtmlEncode(CurrentProduct.ProductName) %>" style="max-width: 100%; max-height: 300px;" />
                            </div>
                            <% } %>
                            
                            <table class="data-table" style="margin-bottom: 20px;">
                                <tr>
                                    <th style="width: 150px;">Category:</th>
                                    <td><%= Server.HtmlEncode(CurrentProduct.CategoryName) %></td>
                                </tr>
                                <tr>
                                    <th>Price:</th>
                                    <td>$<%= CurrentProduct.UnitPrice.ToString("0.00") %></td>
                                </tr>
                                <tr>
                                    <th>In Stock:</th>
                                    <td>
                                        <%= CurrentProduct.UnitsInStock %>
                                        <% if (CurrentProduct.UnitsInStock <= 10) { %>
                                        <span style="color: #e74c3c; font-weight: bold;" title="Low stock!">&nbsp;⚠</span>
                                        <% } %>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Description:</th>
                                    <td><%= Server.HtmlEncode(CurrentProduct.Description) %></td>
                                </tr>
                                <tr>
                                    <th>Created Date:</th>
                                    <td><%= CurrentProduct.CreatedDate.ToString("MM/dd/yyyy hh:mm tt") %></td>
                                </tr>
                            </table>
                            
                            <div>
                                <a href="Products.aspx?action=edit&id=<%= CurrentProduct.ProductID %>" class="btn btn-secondary">Edit Product</a>
                                <a href="Products.aspx" class="btn">Back to List</a>
                                <a href="Products.aspx?action=delete&id=<%= CurrentProduct.ProductID %>" class="btn btn-danger delete-btn">Delete Product</a>
                            </div>
                        </div>
                    </div>
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