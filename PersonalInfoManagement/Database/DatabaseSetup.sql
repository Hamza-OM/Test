-- the database 
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PersonalInfoDB')
BEGIN
    CREATE DATABASE PersonalInfoDB;
END
GO

USE PersonalInfoDB;
GO

-- Drop existing table if exists
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Person')
BEGIN
    DROP TABLE Person;
END
GO

-- Person table
CREATE TABLE Person (
    PersonID INT PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Age INT,
    Gender NVARCHAR(10),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(200),
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- sample data
INSERT INTO Person (PersonID, FirstName, LastName, Age, Gender, Email, Phone, Address)
VALUES 
(1, 'John', 'Doe', 28, 'Male', 'john.doe@example.com', '5551234567', '123 Main St, Anytown'),
(2, 'Jane', 'Smith', 32, 'Female', 'jane.smith@example.com', '5559876543', '456 Oak Ave, Somecity'),
(3, 'Michael', 'Johnson', 45, 'Male', 'michael.j@example.com', '5554567890', '789 Pine Rd, Anothercity');
GO 