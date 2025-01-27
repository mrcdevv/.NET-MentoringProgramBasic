using Microsoft.Data.SqlClient;
using Shop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        private readonly DatabaseHelper _dbHelper;

        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _dbHelper = new DatabaseHelper(connectionString);
        }

        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_dbHelper);
        public IProductRepository Products => _productRepository ??= new ProductRepository(_dbHelper);

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction not started");
            }

            try
            {
                await _transaction.CommitAsync(cancellationToken);
                return 1; //success
            }
            catch
            {
                await _transaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
