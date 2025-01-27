using Microsoft.Data.SqlClient;
using System;
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

                await InsertProductsAsync(connection, cancellationToken);
                await InsertOrdersAsync(connection, cancellationToken);
                await InsertOrderProductsAsync(connection, cancellationToken);
            }
        }

        public static async Task CleanDatabaseAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                await DeleteOrderProductsAsync(connection, cancellationToken);
                await DeleteOrdersAsync(connection, cancellationToken);
                await DeleteProductsAsync(connection, cancellationToken);
            }
        }

        private static async Task DeleteOrderProductsAsync(SqlConnection connection, CancellationToken cancellationToken)
        {
            var query = "DELETE FROM OrderProduct";
            using (var command = new SqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private static async Task DeleteOrdersAsync(SqlConnection connection, CancellationToken cancellationToken)
        {
            var query = "DELETE FROM Orders";
            using (var command = new SqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private static async Task DeleteProductsAsync(SqlConnection connection, CancellationToken cancellationToken)
        {
            var query = "DELETE FROM Product";
            using (var command = new SqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private static async Task InsertProductsAsync(SqlConnection connection, CancellationToken cancellationToken)
        {
            var query = @"
                INSERT INTO Product (Id, Name, Description, Weight, Height, Width, Length)
                VALUES
                (1, 'Laptop', 'High-performance laptop', 2.5, 0.5, 0.3, 0.2),
                (2, 'Smartphone', 'Latest model smartphone', 0.2, 0.15, 0.07, 0.01),
                (3, 'Tablet', '10-inch tablet', 0.5, 0.25, 0.18, 0.08),
                (4, 'Monitor', '27-inch 4K monitor', 5.0, 0.6, 0.7, 0.2),
                (5, 'Keyboard', 'Mechanical keyboard', 1.0, 0.05, 0.4, 0.15);
            ";

            using (var command = new SqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private static async Task InsertOrdersAsync(SqlConnection connection, CancellationToken cancellationToken)
        {
            var query = @"
                INSERT INTO Orders (Id, Status, CreatedDate, UpdatedDate)
                VALUES
                (1, 0, '2023-10-01', '2023-10-01'), -- NotStarted
                (2, 2, '2023-10-02', '2023-10-02'), -- InProgress
                (3, 5, '2023-10-03', '2023-10-03'), -- Cancelled
                (4, 6, '2023-10-04', '2023-10-04'), -- Done
                (5, 1, '2023-10-05', '2023-10-05'); -- Loading
            ";

            using (var command = new SqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private static async Task InsertOrderProductsAsync(SqlConnection connection, CancellationToken cancellationToken)
        {
            var query = @"
                INSERT INTO OrderProduct (OrderId, ProductId)
                VALUES
                (1, 1), -- Orden 1 tiene Laptop
                (1, 2), -- Orden 1 tiene Smartphone
                (2, 3), -- Orden 2 tiene Tablet
                (3, 4), -- Orden 3 tiene Monitor
                (4, 5), -- Orden 4 tiene Keyboard
                (5, 1), -- Orden 5 tiene Laptop
                (5, 3); -- Orden 5 tiene Tablet
            ";

            using (var command = new SqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
}