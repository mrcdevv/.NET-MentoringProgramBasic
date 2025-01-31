using Microsoft.Data.SqlClient;
using Shop.Core.Repositories;
using System;
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
        private readonly DatabaseHelper _dbHelper;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;

        public UnitOfWork(DatabaseHelper dbHelper, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _dbHelper = dbHelper;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public IOrderRepository Orders => _orderRepository;
        public IProductRepository Products => _productRepository;

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction not started");
            }

            try
            {
                await _transaction.CommitAsync(cancellationToken);
                return 1; // Success
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