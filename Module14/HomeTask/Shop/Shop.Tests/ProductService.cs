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
            _productService = new ProductService(_mockProductRepository.Object); // Asume que tienes una clase ProductService que implementa IProductService
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product { Id = productId };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(expectedProduct);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.AreEqual(expectedProduct, result);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var expectedProducts = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.AreEqual(expectedProducts, result);
        }

        [Test]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            var product = new Product { Id = 1 };
            _mockProductRepository.Setup(repo => repo.AddAsync(product)).Returns(Task.CompletedTask);

            // Act
            await _productService.AddAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.AddAsync(product), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product { Id = 1 };
            _mockProductRepository.Setup(repo => repo.UpdateAsync(product)).Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateAsync(product), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteProduct()
        {
            // Arrange
            var productId = 1;
            _mockProductRepository.Setup(repo => repo.DeleteAsync(productId)).Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _mockProductRepository.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }
    }
}