# Product Management System

A simple yet complete ASP.NET web application for product inventory management.

## Features

- **Product Management**: Add, view, edit, and delete products
- **Category Management**: Organize products into categories
- **Low Stock Monitoring**: Track products with low inventory
- **Advanced Search**: Find products with custom filters
- **Inventory Management**: Update stock levels individually or in bulk

## Technology Stack

- **Frontend**: HTML, CSS, JavaScript (with jQuery)
- **Backend**: ASP.NET WebForms (C#)
- **Database**: MS SQL Server
- **Development Environment**: MS Visual Studio

## Setup Instructions

### Database Setup

1. Open SQL Server Management Studio
2. Connect to your local SQL Server instance
3. Run the SQL script located at `Database/CreateDatabase.sql`
   - This script will create the database, tables, and sample data

### Application Setup

1. Open the solution in Visual Studio
2. Check the connection string in `Web.config` and update if necessary
3. Build the solution
4. Press F5 to run the application

## System Requirements

- Visual Studio 2019 or newer
- .NET Framework 4.8
- SQL Server 2016 or newer (Express edition is sufficient)
- IIS Express (included with Visual Studio)

## Project Structure

```
ProductManagementSystem/
├── App_Data/              # Database files and app data
├── Content/               # CSS and static files
├── Controllers/           # Controller logic
├── Database/              # SQL scripts
├── Models/                # Data models
├── Scripts/               # JavaScript files
├── Views/                 # ASPX views
└── Web.config             # Application configuration
```

## Usage

After launching the application, you'll see the home page with a dashboard that displays:
- Total number of products and categories
- Low stock items
- Recently added products

From there, you can:
- Navigate to the Products page to manage products
- Navigate to the Categories page to manage categories
- Use the Low Stock page to quickly update inventory levels
- Use the Search page to find specific products

## Database Schema

The application uses two main tables:

**Categories**
- CategoryID (PK)
- CategoryName
- Description
- CreatedDate

**Products**
- ProductID (PK)
- ProductName
- CategoryID (FK)
- UnitPrice
- UnitsInStock
- Description
- ImagePath
- CreatedDate

## Additional Information

This application demonstrates:
- Database connectivity
- CRUD operations
- Form validation
- Responsive UI design
- AJAX operations
- Advanced search functionality 