-- Create the database
CREATE DATABASE PRN_Assignment;

-- Use the database
USE PRN_Assignment;

-- Create the Member table
CREATE TABLE Member (
    MemberId INT PRIMARY KEY,
    Email VARCHAR(100) NOT NULL,
    CompanyName VARCHAR(40),
    City VARCHAR(15),
    Country VARCHAR(15),
    Password VARCHAR(30) NOT NULL
);

-- Create the Order table
CREATE TABLE [Order] (
    OrderId INT PRIMARY KEY,
    MemberId INT NOT NULL,
    OrderDate DATETIME NOT NULL,
    RequiredDate DATETIME,
    ShippedDate DATETIME,
    Freight MONEY,
    FOREIGN KEY (MemberId) REFERENCES Member(MemberId)
);

-- Create the Product table
CREATE TABLE Product (
    ProductId INT PRIMARY KEY,
    CategoryId INT,
    ProductName VARCHAR(40) NOT NULL,
    Weight VARCHAR(20),
    UnitPrice MONEY NOT NULL,
    UnitsInStock INT
);

-- Create the OrderDetail table
CREATE TABLE OrderDetail (
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    UnitPrice MONEY NOT NULL,
    Quantity INT NOT NULL,
    Discount FLOAT,
    PRIMARY KEY (OrderId, ProductId),
    FOREIGN KEY (OrderId) REFERENCES [Order](OrderId),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);

-- Use the database
USE PRN_Assignment;

-- Insert sample data into Member table
INSERT INTO Member (MemberId, Email, CompanyName, City, Country, Password) VALUES
(1,'john.doe@example.com', 'Doe Enterprises', 'New York', 'USA', 'pass123'),
(2, 'jane.smith@example.com', 'Smith Co', 'Los Angeles', 'USA', 'pass123'),
(3, 'will.jones@example.com', 'Jones Corp', 'Chicago', 'USA', 'pass123'),
(4, 'alice.brown@example.com', 'Brown Ltd', 'Houston', 'USA', 'pass123'),
(5, 'charles.johnson@example.com', 'Johnson Inc', 'Phoenix', 'USA', 'pass123'),
(6, 'emily.davis@example.com', 'Davis LLC', 'Philadelphia', 'USA', 'pass123'),
(7, 'michael.miller@example.com', 'Miller Group', 'San Antonio', 'USA', 'pass123'),
(8, 'sarah.wilson@example.com', 'Wilson & Sons', 'San Diego', 'USA', 'pass123'),
(9, 'david.moore@example.com', 'Moore Partners', 'Dallas', 'USA', 'pass123'),
(10, 'linda.taylor@example.com', 'Taylor Ventures', 'San Jose', 'USA', 'pass123');

-- Insert sample data into Order table
INSERT INTO [Order] (OrderId, MemberId, OrderDate, RequiredDate, ShippedDate, Freight) VALUES
(1, 1, '2024-01-10', '2024-01-15', '2024-01-14', 15.00),
(2, 2, '2024-01-11', '2024-01-16', '2024-01-15', 25.00),
(3, 3, '2024-01-12', '2024-01-17', '2024-01-16', 10.00),
(4, 4, '2024-01-13', '2024-01-18', '2024-01-17', 20.00),
(5, 5, '2024-01-14', '2024-01-19', '2024-01-18', 30.00),
(6, 6, '2024-01-15', '2024-01-20', '2024-01-19', 5.00),
(7, 7, '2024-01-16', '2024-01-21', '2024-01-20', 12.00),
(8, 8, '2024-01-17', '2024-01-22', '2024-01-21', 8.00),
(9, 9, '2024-01-18', '2024-01-23', '2024-01-22', 18.00),
(10, 10, '2024-01-19', '2024-01-24', '2024-01-23', 22.00);

-- Insert sample data into Product table
INSERT INTO Product (ProductId, CategoryId, ProductName, Weight, UnitPrice, UnitsInStock) VALUES
(1, 1, 'Laptop', '2.5 kg', 1500.00, 50),
(2, 2, 'Smartphone', '0.2 kg', 800.00, 200),
(3, 1, 'Tablet', '0.5 kg', 600.00, 150),
(4, 3, 'Monitor', '5 kg', 300.00, 75),
(5, 2, 'Smartwatch', '0.1 kg', 200.00, 300),
(6, 4, 'Headphones', '0.3 kg', 150.00, 100),
(7, 3, 'Keyboard', '0.8 kg', 50.00, 120),
(8, 4, 'Mouse', '0.1 kg', 30.00, 250),
(9, 5, 'Printer', '6 kg', 200.00, 40),
(10, 5, 'Scanner', '4 kg', 250.00, 60);

-- Insert sample data into OrderDetail table
INSERT INTO OrderDetail (OrderId, ProductId, UnitPrice, Quantity, Discount) VALUES
(1, 1, 1500.00, 2, 0.10),
(1, 2, 800.00, 1, 0.05),
(2, 3, 600.00, 3, 0.00),
(2, 4, 300.00, 2, 0.05),
(3, 5, 200.00, 5, 0.10),
(3, 6, 150.00, 4, 0.00),
(4, 7, 50.00, 10, 0.10),
(4, 8, 30.00, 8, 0.05),
(5, 9, 200.00, 1, 0.00),
(5, 10, 250.00, 2, 0.00),
(6, 1, 1500.00, 1, 0.05),
(6, 2, 800.00, 2, 0.00),
(7, 3, 600.00, 1, 0.10),
(7, 4, 300.00, 3, 0.05),
(8, 5, 200.00, 2, 0.00),
(8, 6, 150.00, 1, 0.10),
(9, 7, 50.00, 5, 0.05),
(9, 8, 30.00, 4, 0.00),
(10, 9, 200.00, 2, 0.00),
(10, 10, 250.00, 1, 0.05);
