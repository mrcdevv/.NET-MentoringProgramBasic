using Shop.Core.Models;
using Shop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Core.Services
{
    public interface IOrderService
    {
        Task<Order> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteBulkAsync(int[] orderIds, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId, CancellationToken cancellationToken = default);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _orderRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _orderRepository.GetAllAsync(cancellationToken);
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderRepository.AddAsync(order, cancellationToken);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orderRepository.UpdateAsync(order, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _orderRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task DeleteBulkAsync(int[] orderIds, CancellationToken cancellationToken = default)
        {
            await _orderRepository.DeleteBulkAsync(orderIds, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId, CancellationToken cancellationToken = default)
        {
            return await _orderRepository.GetOrdersFilteredAsync(year, month, status, productId, cancellationToken);
        }
    }
}