using Moq;
using Shop.Core.Models;
using Shop.Core.Repositories;
using Shop.Core.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product { Id = productId, Name = "Laptop" };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProduct));
            _mockProductRepository.Verify(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop" },
                new Product { Id = 2, Name = "Smartphone" }
            };
            _mockProductRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
            _mockProductRepository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Laptop" };
            _mockProductRepository.Setup(repo => repo.AddAsync(product, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.AddAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.AddAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Laptop" };
            _mockProductRepository.Setup(repo => repo.UpdateAsync(product, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteProduct()
        {
            // Arrange
            var productId = 1;
            _mockProductRepository.Setup(repo => repo.DeleteAsync(productId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _mockProductRepository.Verify(repo => repo.DeleteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}