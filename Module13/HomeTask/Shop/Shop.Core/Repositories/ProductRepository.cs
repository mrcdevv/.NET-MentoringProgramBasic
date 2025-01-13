using Microsoft.Data.SqlClient;
using Shop.Core.Data;
using Shop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public ProductRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand("SELECT * FROM Product WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Weight = reader.GetDecimal(3),
                            Height = reader.GetDecimal(4),
                            Width = reader.GetDecimal(5),
                            Length = reader.GetDecimal(6)
                        };
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand("SELECT * FROM Product", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Weight = reader.GetDecimal(3),
                            Height = reader.GetDecimal(4),
                            Width = reader.GetDecimal(5),
                            Length = reader.GetDecimal(6)
                        });
                    }
                }
            }
            return products;
        }

        public async Task AddAsync(Product product)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand(
                    "INSERT INTO Product (Id, Name, Description, Weight, Height, Width, Length) " +
                    "VALUES (@Id, @Name, @Description, @Weight, @Height, @Width, @Length)", connection);
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Weight", product.Weight);
                command.Parameters.AddWithValue("@Height", product.Height);
                command.Parameters.AddWithValue("@Width", product.Width);
                command.Parameters.AddWithValue("@Length", product.Length);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Product product)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var command = new SqlCommand(
                    "UPDATE Product SET Name = @Name, Description = @Description, Weight = @Weight, " +
                    "Height = @Height, Width = @Width, Length = @Length WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Weight", product.Weight);
                command.Parameters.AddWithValue("@Height", product.Height);
                command.Parameters.AddWithValue("@Width", product.Width);
                command.Parameters.AddWithValue("@Length", product.Length);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var deleteOrderProductCommand = new SqlCommand("DELETE FROM OrderProduct WHERE ProductId = @ProductId", connection);
                deleteOrderProductCommand.Parameters.AddWithValue("@ProductId", id);
                await deleteOrderProductCommand.ExecuteNonQueryAsync();

                var deleteProductCommand = new SqlCommand("DELETE FROM Product WHERE Id = @Id", connection);
                deleteProductCommand.Parameters.AddWithValue("@Id", id);
                await deleteProductCommand.ExecuteNonQueryAsync();
            }
        }
    }


}
