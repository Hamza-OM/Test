# Product Management System

A simple ASP.NET web application with database CRUD functionality for managing products and categories.

## Technologies Used

- ASP.NET MVC
- MS SQL Server
- HTML, CSS, JavaScript
- Bootstrap for responsive UI

## Database Setup

1. Open SQL Server Management Studio
2. Connect to your local SQL Server instance
3. Run the script in `Database/DatabaseSetup.sql` to create the database and tables

## Application Setup

1. Open the solution in Visual Studio
2. Update the connection string in `Web.config` to match your SQL Server instance
3. Build the solution
4. Run the application

## Features

- **Dashboard**: View summary of products, categories, and low stock items
- **Products Management**:
  - View all products with search and filtering capabilities
  - Add new products
  - Edit existing products
  - Delete products
  - View low stock products
- **Categories Management**:
  - View all categories
  - Add new categories
  - Edit existing categories
  - Delete categories (only if they don't contain products)

## Usage Guide

1. **Home Page**: The dashboard shows summary statistics of your inventory
2. **Products**: View, search, filter, and manage your products
3. **Categories**: Organize your products into categories
4. **Low Stock**: Quickly identify products with low inventory levels

## Requirements

- Visual Studio 2019 or newer
- .NET Framework 4.7.2 or newer
- SQL Server (Express Edition is sufficient) 