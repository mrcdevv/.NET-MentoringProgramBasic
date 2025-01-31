using Moq;
using Shop.Core.Models;
using Shop.Core.Repositories;
using Shop.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _mockOrderRepository;
        private IOrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _orderService = new OrderService(_mockOrderRepository.Object);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnOrder()
        {
            // Arrange
            var orderId = 1;
            var token = CancellationToken.None;
            var expectedOrder = new Order { Id = orderId };
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId, token)).ReturnsAsync(expectedOrder);

            // Act
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedOrder));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllOrders()
        {
            // Arrange
            var token = CancellationToken.None;
            var expectedOrders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };
            _mockOrderRepository.Setup(repo => repo.GetAllAsync(token)).ReturnsAsync(expectedOrders);

            // Act
            var result = await _orderService.GetAllAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedOrders));
        }

        [Test]
        public async Task AddAsync_ShouldAddOrder()
        {
            // Arrange
            var token = CancellationToken.None;
            var order = new Order { Id = 1 };
            _mockOrderRepository.Setup(repo => repo.AddAsync(order, token)).Returns(Task.CompletedTask);

            // Act
            await _orderService.AddAsync(order);

            // Assert
            _mockOrderRepository.Verify(repo => repo.AddAsync(order, token), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateOrder()
        {
            // Arrange
            var token = CancellationToken.None;
            var order = new Order { Id = 1 };
            _mockOrderRepository.Setup(repo => repo.UpdateAsync(order, token)).Returns(Task.CompletedTask);

            // Act
            await _orderService.UpdateAsync(order);

            // Assert
            _mockOrderRepository.Verify(repo => repo.UpdateAsync(order, token), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteOrder()
        {
            // Arrange
            var orderId = 1;
            var token = CancellationToken.None;
            _mockOrderRepository.Setup(repo => repo.DeleteAsync(orderId, token)).Returns(Task.CompletedTask);

            // Act
            await _orderService.DeleteAsync(orderId);

            // Assert
            _mockOrderRepository.Verify(repo => repo.DeleteAsync(orderId, token), Times.Once);
        }

        [Test]
        public async Task DeleteBulkAsync_ShouldDeleteOrdersInBulk()
        {
            // Arrange
            var orderIds = new int[] { 1, 2, 3 };
            var token = CancellationToken.None;
            _mockOrderRepository.Setup(repo => repo.DeleteBulkAsync(orderIds, token)).Returns(Task.CompletedTask);

            // Act
            await _orderService.DeleteBulkAsync(orderIds);

            // Assert
            _mockOrderRepository.Verify(repo => repo.DeleteBulkAsync(orderIds, token), Times.Once);
        }

        [Test]
        public async Task GetOrdersFilteredAsync_ShouldReturnFilteredOrders()
        {
            // Arrange
            var token = CancellationToken.None;
            var year = 2023;
            var month = 10;
            var status = OrderStatus.InProgress;
            var productId = 1;
            var expectedOrders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };
            _mockOrderRepository.Setup(repo => repo.GetOrdersFilteredAsync(year, month, status, productId, token)).ReturnsAsync(expectedOrders);

            // Act
            var result = await _orderService.GetOrdersFilteredAsync(year, month, status, productId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedOrders));
        }
    }
}
