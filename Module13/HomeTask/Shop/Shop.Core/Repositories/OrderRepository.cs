using Microsoft.Data.SqlClient;
using Shop.Core.Data;
using Shop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task DeleteBulkAsync(int[] orderIds);
        Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public OrderRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand("SELECT * FROM Orders WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Order
                        {
                            Id = reader.GetInt32(0),
                            Status = (OrderStatus)reader.GetInt32(1),
                            CreatedDate = reader.GetDateTime(2),
                            UpdatedDate = reader.GetDateTime(3)
                        };
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = new List<Order>();
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand("SELECT * FROM Orders", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        orders.Add(new Order
                        {
                            Id = reader.GetInt32(0),
                            Status = (OrderStatus)reader.GetInt32(1),
                            CreatedDate = reader.GetDateTime(2),
                            UpdatedDate = reader.GetDateTime(3)
                        });
                    }
                }
            }
            return orders;
        }

        public async Task AddAsync(Order order)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand(
                    "INSERT INTO Orders (Id, Status, CreatedDate, UpdatedDate) " +
                    "VALUES (@Id, @Status, @CreatedDate, @UpdatedDate)", connection);
                command.Parameters.AddWithValue("@Id", order.Id);
                command.Parameters.AddWithValue("@Status", (int)order.Status);
                command.Parameters.AddWithValue("@CreatedDate", order.CreatedDate);
                command.Parameters.AddWithValue("@UpdatedDate", order.UpdatedDate);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Order order)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand(
                    "UPDATE Orders SET Status = @Status, CreatedDate = @CreatedDate, " +
                    "UpdatedDate = @UpdatedDate WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", order.Id);
                command.Parameters.AddWithValue("@Status", (int)order.Status);
                command.Parameters.AddWithValue("@CreatedDate", order.CreatedDate);
                command.Parameters.AddWithValue("@UpdatedDate", order.UpdatedDate);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var deleteOrderProductCommand = new SqlCommand("DELETE FROM OrderProduct WHERE OrderId = @OrderId", connection);
                deleteOrderProductCommand.Parameters.AddWithValue("@OrderId", id);
                await deleteOrderProductCommand.ExecuteNonQueryAsync();

                var deleteOrderCommand = new SqlCommand("DELETE FROM Orders WHERE Id = @Id", connection);
                deleteOrderCommand.Parameters.AddWithValue("@Id", id);
                await deleteOrderCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteBulkAsync(int[] orderIds)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteOrderProductCommand = new SqlCommand("DELETE FROM OrderProduct WHERE OrderId IN (@OrderIds)", connection, transaction);
                        deleteOrderProductCommand.Parameters.AddWithValue("@OrderIds", string.Join(",", orderIds));
                        await deleteOrderProductCommand.ExecuteNonQueryAsync();

                        var deleteOrderCommand = new SqlCommand("DELETE FROM Orders WHERE Id IN (@OrderIds)", connection, transaction);
                        deleteOrderCommand.Parameters.AddWithValue("@OrderIds", string.Join(",", orderIds));
                        await deleteOrderCommand.ExecuteNonQueryAsync();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId)
        {
            var orders = new List<Order>();
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand("GetFilteredOrders", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Year", year ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Month", month ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Status", status.HasValue ? (int)status.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProductId", productId ?? (object)DBNull.Value);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        orders.Add(new Order
                        {
                            Id = reader.GetInt32(0),
                            Status = (OrderStatus)reader.GetInt32(1),
                            CreatedDate = reader.GetDateTime(2),
                            UpdatedDate = reader.GetDateTime(3)
                        });
                    }
                }
            }
            return orders;
        }
    }
}
