-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PersonalInfoDB')
BEGIN
    CREATE DATABASE PersonalInfoDB;
END
GO

USE PersonalInfoDB;
GO

-- Create Person table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Person')
BEGIN
    CREATE TABLE Person (
        PersonID INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Age INT,
        Gender NVARCHAR(10),
        Email NVARCHAR(100),
        Phone NVARCHAR(20),
        Address NVARCHAR(200),
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- Insert sample data
IF (SELECT COUNT(*) FROM Person) = 0
BEGIN
    INSERT INTO Person (FirstName, LastName, Age, Gender, Email, Phone, Address)
    VALUES 
    ('John', 'Doe', 28, 'Male', 'john.doe@example.com', '5551234567', '123 Main St, Anytown'),
    ('Jane', 'Smith', 32, 'Female', 'jane.smith@example.com', '5559876543', '456 Oak Ave, Somecity'),
    ('Michael', 'Johnson', 45, 'Male', 'michael.j@example.com', '5554567890', '789 Pine Rd, Anothercity');
END
GO 