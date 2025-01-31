using Shop.Core.Repositories;
using Shop.Core.Services;
using Shop.Core.Models;
using Moq;

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
            var token = CancellationToken.None;
            var expectedProduct = new Product { Id = productId };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId, token)).ReturnsAsync(expectedProduct);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProduct));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var token = CancellationToken.None;
            var expectedProducts = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            _mockProductRepository.Setup(repo => repo.GetAllAsync(token)).ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        [Test]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            var token = CancellationToken.None;
            var product = new Product { Id = 1 };
            _mockProductRepository.Setup(repo => repo.AddAsync(product, token)).Returns(Task.CompletedTask);

            // Act
            await _productService.AddAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.AddAsync(product, token), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            // Arrange
            var token = CancellationToken.None;
            var product = new Product { Id = 1 };
            _mockProductRepository.Setup(repo => repo.UpdateAsync(product, token)).Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateAsync(product, token), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteProduct()
        {
            // Arrange
            var token = CancellationToken.None;
            var productId = 1;
            _mockProductRepository.Setup(repo => repo.DeleteAsync(productId, token)).Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _mockProductRepository.Verify(repo => repo.DeleteAsync(productId, token), Times.Once);
        }
    }
}