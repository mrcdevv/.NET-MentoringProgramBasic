using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        private readonly ShopDbContext _context;

        public OrderRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteBulkAsync(int[] orderIds, CancellationToken cancellationToken = default)
        {
            var orders = await _context.Orders
                .Where(o => orderIds.Contains(o.Id))
                .Include(o => o.OrderProducts)
                .ToListAsync(cancellationToken);

            _context.Orders.RemoveRange(orders);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId, CancellationToken cancellationToken = default)
        {
            var query = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .AsQueryable();

            if (year.HasValue)
                query = query.Where(o => o.CreatedDate.Year == year.Value);

            if (month.HasValue)
                query = query.Where(o => o.CreatedDate.Month == month.Value);

            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            if (productId.HasValue)
                query = query.Where(o => o.OrderProducts.Any(op => op.ProductId == productId.Value));

            return await query.ToListAsync(cancellationToken);
        }
    }
}
