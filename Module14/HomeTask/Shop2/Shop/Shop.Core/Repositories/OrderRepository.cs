using Dapper;
using Shop.Core.Data;
using Shop.Core.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Core.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteBulkAsync(int[] orderIds, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId, CancellationToken cancellationToken = default);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public OrderRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<Order> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = "SELECT * FROM Orders WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = "SELECT * FROM Orders";
                return await connection.QueryAsync<Order>(sql);
            }
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = """
                    INSERT INTO Orders (Id, Status, CreatedDate, UpdatedDate) 
                    VALUES (@Id, @Status, @CreatedDate, @UpdatedDate)
                    """;
                await connection.ExecuteAsync(sql, order);
            }
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = """
                    UPDATE Orders SET Status = @Status, CreatedDate = @CreatedDate, 
                    UpdatedDate = @UpdatedDate WHERE Id = @Id
                    """;
                await connection.ExecuteAsync(sql, order);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var deleteOrderProductSql = "DELETE FROM OrderProduct WHERE OrderId = @OrderId";
                await connection.ExecuteAsync(deleteOrderProductSql, new { OrderId = id });

                var deleteOrderSql = "DELETE FROM Orders WHERE Id = @Id";
                await connection.ExecuteAsync(deleteOrderSql, new { Id = id });
            }
        }

        public async Task DeleteBulkAsync(int[] orderIds, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = "DELETE FROM Orders WHERE Id IN @OrderIds";
                await connection.ExecuteAsync(sql, new { OrderIds = orderIds });
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = "EXEC GetFilteredOrders @Year, @Month, @Status, @ProductId";
                return await connection.QueryAsync<Order>(
                    sql, new { Year = year, Month = month, Status = status, ProductId = productId });
            }
        }
    }
}