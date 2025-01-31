using Dapper;
using Microsoft.Data.SqlClient;
using Shop.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Core.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var products = new List<Product>
                {
                    new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Weight = 2.5m, Height = 0.5m, Width = 0.3m, Length = 0.2m },
                    new Product { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Weight = 0.2m, Height = 0.15m, Width = 0.07m, Length = 0.01m },
                    new Product { Id = 3, Name = "Tablet", Description = "10-inch tablet", Weight = 0.5m, Height = 0.25m, Width = 0.18m, Length = 0.08m },
                    new Product { Id = 4, Name = "Monitor", Description = "27-inch 4K monitor", Weight = 5.0m, Height = 0.6m, Width = 0.7m, Length = 0.2m },
                    new Product { Id = 5, Name = "Keyboard", Description = "Mechanical keyboard", Weight = 1.0m, Height = 0.05m, Width = 0.4m, Length = 0.15m }
                };

                foreach (var product in products)
                {
                    var sql = """
                        IF NOT EXISTS (SELECT 1 FROM Product WHERE Id = @Id)
                        BEGIN
                            INSERT INTO Product (Id, Name, Description, Weight, Height, Width, Length) 
                            VALUES (@Id, @Name, @Description, @Weight, @Height, @Width, @Length)
                        END
                        """;
                    await connection.ExecuteAsync(sql, product);
                }

                var orders = new List<Order>
                {
                    new Order { Id = 1, Status = OrderStatus.NotStarted, CreatedDate = new DateTime(2023, 10, 1), UpdatedDate = new DateTime(2023, 10, 1) },
                    new Order { Id = 2, Status = OrderStatus.InProgress, CreatedDate = new DateTime(2023, 10, 2), UpdatedDate = new DateTime(2023, 10, 2) },
                    new Order { Id = 3, Status = OrderStatus.Cancelled, CreatedDate = new DateTime(2023, 10, 3), UpdatedDate = new DateTime(2023, 10, 3) },
                    new Order { Id = 4, Status = OrderStatus.Done, CreatedDate = new DateTime(2023, 10, 4), UpdatedDate = new DateTime(2023, 10, 4) },
                    new Order { Id = 5, Status = OrderStatus.Loading, CreatedDate = new DateTime(2023, 10, 5), UpdatedDate = new DateTime(2023, 10, 5) }
                };

                foreach (var order in orders)
                {
                    var sql = """
                        IF NOT EXISTS (SELECT 1 FROM Orders WHERE Id = @Id)
                        BEGIN
                            INSERT INTO Orders (Id, Status, CreatedDate, UpdatedDate) 
                            VALUES (@Id, @Status, @CreatedDate, @UpdatedDate)
                        END
                        """;
                    await connection.ExecuteAsync(sql, order);
                }

                var orderProducts = new List<OrderProduct>
                {
                    new OrderProduct { OrderId = 1, ProductId = 1 },
                    new OrderProduct { OrderId = 1, ProductId = 2 },
                    new OrderProduct { OrderId = 2, ProductId = 3 },
                    new OrderProduct { OrderId = 3, ProductId = 4 },
                    new OrderProduct { OrderId = 4, ProductId = 5 },
                    new OrderProduct { OrderId = 5, ProductId = 1 },
                    new OrderProduct { OrderId = 5, ProductId = 3 }
                };

                foreach (var orderProduct in orderProducts)
                {
                    var sql = """
                        IF NOT EXISTS (SELECT 1 FROM OrderProduct WHERE OrderId = @OrderId AND ProductId = @ProductId)
                        BEGIN
                            INSERT INTO OrderProduct (OrderId, ProductId) 
                            VALUES (@OrderId, @ProductId)
                        END
                        """;
                    await connection.ExecuteAsync(sql, orderProduct);
                }
            }
        }

        public static async Task CleanDatabaseAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                await connection.ExecuteAsync("DELETE FROM OrderProduct", cancellationToken);
                await connection.ExecuteAsync("DELETE FROM Orders", cancellationToken);
                await connection.ExecuteAsync("DELETE FROM Product", cancellationToken);
            }
        }
    }
}