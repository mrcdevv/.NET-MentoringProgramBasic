using Dapper;
using Shop.Core.Data;
using Shop.Core.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public ProductRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = "SELECT * FROM Product WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = "SELECT * FROM Product";
                return await connection.QueryAsync<Product>(sql);
            }
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = """
                    INSERT INTO Product (Id, Name, Description, Weight, Height, Width, Length) 
                    VALUES (@Id, @Name, @Description, @Weight, @Height, @Width, @Length)
                    """;
                await connection.ExecuteAsync(sql, product);
            }
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var sql = """
                    UPDATE Product SET Name = @Name, Description = @Description, 
                    Weight = @Weight, Height = @Height, Width = @Width, Length = @Length 
                    WHERE Id = @Id
                    """;
                await connection.ExecuteAsync(sql, product);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using (var connection = await _dbHelper.GetConnectionAsync())
            {
                var deleteOrderProductSql = "DELETE FROM OrderProduct WHERE ProductId = @ProductId";
                await connection.ExecuteAsync(deleteOrderProductSql, new { ProductId = id });

                var deleteProductSql = "DELETE FROM Product WHERE Id = @Id";
                await connection.ExecuteAsync(deleteProductSql, new { Id = id });
            }
        }
    }
}