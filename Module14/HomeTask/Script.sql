USE master;
GO

-- Crear la base de datos Store si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Store')
BEGIN
    CREATE DATABASE Store;
END
GO

USE Store;
GO

-- Crear la tabla Product
CREATE TABLE Product (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100),
    Description NVARCHAR(MAX),
    Weight DECIMAL(10, 2),
    Height DECIMAL(10, 2),
    Width DECIMAL(10, 2),
    Length DECIMAL(10, 2)
);
GO

-- Crear la tabla Orders
CREATE TABLE Orders (
    Id INT PRIMARY KEY,
    Status INT,
    CreatedDate DATETIME,
    UpdatedDate DATETIME
);
GO

-- Crear la tabla OrderProduct
CREATE TABLE OrderProduct (
    OrderId INT,
    ProductId INT,
    PRIMARY KEY (OrderId, ProductId),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);
GO

-- Crear el procedimiento almacenado GetFilteredOrders
CREATE PROCEDURE GetFilteredOrders
    @Year INT = NULL,
    @Month INT = NULL,
    @Status INT = NULL,
    @ProductId INT = NULL
AS
BEGIN
    SELECT o.*
    FROM Orders o
    LEFT JOIN OrderProduct op ON o.Id = op.OrderId
    WHERE (@Year IS NULL OR YEAR(o.CreatedDate) = @Year)
      AND (@Month IS NULL OR MONTH(o.CreatedDate) = @Month)
      AND (@Status IS NULL OR o.Status = @Status)
      AND (@ProductId IS NULL OR op.ProductId = @ProductId);
END
GO

-- Insertar productos
INSERT INTO Product (Id, Name, Description, Weight, Height, Width, Length)
VALUES
(1, 'Laptop', 'High-performance laptop', 2.5, 0.5, 0.3, 0.2),
(2, 'Smartphone', 'Latest model smartphone', 0.2, 0.15, 0.07, 0.01),
(3, 'Tablet', '10-inch tablet', 0.5, 0.25, 0.18, 0.08),
(4, 'Monitor', '27-inch 4K monitor', 5.0, 0.6, 0.7, 0.2),
(5, 'Keyboard', 'Mechanical keyboard', 1.0, 0.05, 0.4, 0.15);
GO

-- Insertar órdenes
INSERT INTO Orders (Id, Status, CreatedDate, UpdatedDate)
VALUES
(1, 0, '2023-10-01', '2023-10-01'), -- NotStarted
(2, 2, '2023-10-02', '2023-10-02'), -- InProgress
(3, 5, '2023-10-03', '2023-10-03'), -- Cancelled
(4, 6, '2023-10-04', '2023-10-04'), -- Done
(5, 1, '2023-10-05', '2023-10-05'); -- Loading
GO

-- Establecer relación muchos a muchos entre órdenes y productos
INSERT INTO OrderProduct (OrderId, ProductId)
VALUES
(1, 1), -- Orden 1 tiene Laptop
(1, 2), -- Orden 1 tiene Smartphone
(2, 3), -- Orden 2 tiene Tablet
(3, 4), -- Orden 3 tiene Monitor
(4, 5), -- Orden 4 tiene Keyboard
(5, 1), -- Orden 5 tiene Laptop
(5, 3); -- Orden 5 tiene Tablet
GO