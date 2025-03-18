-- Create Database Script for Product Management System

-- Create the database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ProductManagementDB')
BEGIN
    CREATE DATABASE ProductManagementDB;
END
GO

USE ProductManagementDB;
GO

-- Create Categories table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        CategoryID INT PRIMARY KEY IDENTITY(1,1),
        CategoryName NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- Create Products table with foreign key reference to Categories
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        ProductID INT PRIMARY KEY IDENTITY(1,1),
        ProductName NVARCHAR(100) NOT NULL,
        CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
        UnitPrice DECIMAL(10, 2) NOT NULL,
        UnitsInStock INT NOT NULL DEFAULT 0,
        Description NVARCHAR(500) NULL,
        ImagePath NVARCHAR(200) NULL,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- Insert sample data into Categories table
IF NOT EXISTS (SELECT TOP 1 * FROM Categories)
BEGIN
    INSERT INTO Categories (CategoryName, Description)
    VALUES 
        ('Electronics', 'Electronic devices and accessories'),
        ('Clothing', 'Apparel and fashion accessories'),
        ('Books', 'Books, magazines and publications'),
        ('Home & Garden', 'Home decor and gardening supplies');
END
GO

-- Insert sample data into Products table
IF NOT EXISTS (SELECT TOP 1 * FROM Products)
BEGIN
    INSERT INTO Products (ProductName, CategoryID, UnitPrice, UnitsInStock, Description)
    VALUES 
        ('Smartphone', 1, 599.99, 50, 'Latest smartphone with advanced features'),
        ('Laptop', 1, 899.99, 25, 'High-performance laptop for work and gaming'),
        ('T-shirt', 2, 19.99, 100, 'Cotton t-shirt, available in multiple colors'),
        ('Jeans', 2, 39.99, 75, 'Denim jeans, slim fit'),
        ('Novel', 3, 14.99, 200, 'Bestselling fiction novel'),
        ('Cookbook', 3, 24.99, 30, 'Collection of gourmet recipes'),
        ('Plant Pot', 4, 9.99, 150, 'Ceramic plant pot for indoor plants'),
        ('Garden Tools Set', 4, 49.99, 20, 'Complete set of essential garden tools');
END
GO

PRINT 'Database setup completed successfully.';
GO 