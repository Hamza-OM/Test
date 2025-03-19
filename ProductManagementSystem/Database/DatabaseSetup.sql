-- Create database
CREATE DATABASE ProductManagementDB;
GO

USE ProductManagementDB;
GO

-- Create Categories table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200)
);
GO

-- Create Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    Price DECIMAL(10, 2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    Description NVARCHAR(500)
);
GO

-- Insert sample categories
INSERT INTO Categories (CategoryName, Description)
VALUES 
    ('Electronics', 'Electronic devices and gadgets'),
    ('Clothing', 'Apparel and fashion items'),
    ('Books', 'Books and publications');
GO

-- Insert sample products
INSERT INTO Products (ProductName, CategoryID, Price, StockQuantity, Description)
VALUES
    ('Smartphone', 1, 599.99, 50, 'Latest smartphone model'),
    ('Laptop', 1, 999.99, 30, 'High-performance laptop'),
    ('T-Shirt', 2, 19.99, 100, 'Cotton t-shirt'),
    ('Jeans', 2, 49.99, 75, 'Denim jeans'),
    ('Programming Book', 3, 39.99, 25, 'Learn programming book');
GO 