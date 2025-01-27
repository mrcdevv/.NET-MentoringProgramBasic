USE master;
GO

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

CREATE TABLE Orders (
    Id INT PRIMARY KEY,
    Status INT,
    CreatedDate DATETIME,
    UpdatedDate DATETIME
);
GO

CREATE TABLE OrderProduct (
    OrderId INT,
    ProductId INT,
    PRIMARY KEY (OrderId, ProductId),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);
GO

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
