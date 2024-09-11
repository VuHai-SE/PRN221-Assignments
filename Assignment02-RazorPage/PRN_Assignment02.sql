-- Create the database
CREATE DATABASE PRN_Assignment02;

-- Use the database
USE PRN_Assignment02;

-- Create the Member table
CREATE TABLE Member (
    MemberId INT PRIMARY KEY IDENTITY,
    Email VARCHAR(100) NOT NULL,
    CompanyName VARCHAR(40),
    City VARCHAR(15),
    Country VARCHAR(15),
    Password VARCHAR(100) NOT NULL
);

-- Create the Order table
CREATE TABLE [Order] (
    OrderId INT PRIMARY KEY IDENTITY,
    MemberId INT NOT NULL,
    OrderDate DATETIME NOT NULL,
    RequiredDate DATETIME,
    ShippedDate DATETIME,
    Freight MONEY,
    FOREIGN KEY (MemberId) REFERENCES Member(MemberId)
);

-- Create the Product table
CREATE TABLE Product (
    ProductId INT PRIMARY KEY IDENTITY,
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
USE PRN_Assignment02;

-- Insert sample data into Member table (pass là: 123pass)
INSERT INTO Member (Email, CompanyName, City, Country, Password) VALUES
('haivv@gmail.com', 'Vu Van Hai', 'Ha Noi', 'VN', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'), 
('jane.smith@example.com', 'Smith Co', 'Los Angeles', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'),
('will.jones@example.com', 'Jones Corp', 'Chicago', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'),
('alice.brown@example.com', 'Brown Ltd', 'Houston', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'),
('charles.johnson@example.com', 'Johnson Inc', 'Phoenix', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'),
('emily.davis@example.com', 'Davis LLC', 'Philadelphia', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'),
('michael.miller@example.com', 'Miller Group', 'San Antonio', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'),
('sarah.wilson@example.com', 'Wilson & Sons', 'San Diego', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'),
('david.moore@example.com', 'Moore Partners', 'Dallas', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11'),
('linda.taylor@example.com', 'Taylor Ventures', 'San Jose', 'USA', 'e6baa8a460cc15a544c61e976eae8a7d848408ce1a79db696334e54eb41b6b11');

-- Insert sample data into Order table
INSERT INTO [Order] (MemberId, OrderDate, RequiredDate, ShippedDate, Freight) VALUES
(1, '2024-01-10', '2024-01-15', '2024-01-14', 15.00),
(2, '2024-01-11', '2024-01-16', '2024-01-15', 25.00),
(3, '2024-01-12', '2024-01-17', '2024-01-16', 10.00),
(4, '2024-01-13', '2024-01-18', '2024-01-17', 20.00),
(5, '2024-01-14', '2024-01-19', '2024-01-18', 30.00),
(6, '2024-01-15', '2024-01-20', '2024-01-19', 5.00),
(7, '2024-01-16', '2024-01-21', '2024-01-20', 12.00),
(8, '2024-01-17', '2024-01-22', '2024-01-21', 8.00),
(9, '2024-01-18', '2024-01-23', '2024-01-22', 18.00),
(10, '2024-01-19', '2024-01-24', '2024-01-23', 22.00);

-- Insert sample data into Product table
INSERT INTO Product (CategoryId, ProductName, Weight, UnitPrice, UnitsInStock) VALUES
(1, 'Laptop', '2.5 kg', 1500.00, 50),
(2, 'Smartphone', '0.2 kg', 800.00, 200),
(1, 'Tablet', '0.5 kg', 600.00, 150),
(3, 'Monitor', '5 kg', 300.00, 75),
(2, 'Smartwatch', '0.1 kg', 200.00, 300),
(4, 'Headphones', '0.3 kg', 150.00, 100),
(3, 'Keyboard', '0.8 kg', 50.00, 120),
(4, 'Mouse', '0.1 kg', 30.00, 250),
(5, 'Printer', '6 kg', 200.00, 40),
(5, 'Scanner', '4 kg', 250.00, 60);

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
