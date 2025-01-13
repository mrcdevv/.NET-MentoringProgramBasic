using Shop.Core.Models;
using Shop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Services
{
    public interface IOrderService
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task DeleteBulkAsync(int[] orderIds);
        Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
        }

        public async Task UpdateAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task DeleteBulkAsync(int[] orderIds)
        {
            await _orderRepository.DeleteBulkAsync(orderIds);
        }

        public async Task<IEnumerable<Order>> GetOrdersFilteredAsync(int? year, int? month, OrderStatus? status, int? productId)
        {
            return await _orderRepository.GetOrdersFilteredAsync(year, month, status, productId);
        }
    }
}
