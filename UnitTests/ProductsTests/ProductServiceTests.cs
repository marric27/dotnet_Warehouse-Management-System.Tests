using Xunit;
using Moq;
using FluentAssertions;
using dotnet_Warehouse_Management_System.Products.Entities.Services;
using dotnet_Warehouse_Management_System.Products.Entities.Repository;
using dotnet_Warehouse_Management_System.Products.Entities.Dtos;
using dotnet_Warehouse_Management_System.Products.Entities.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_Warehouse_Management_System.Products.Entities;

namespace dotnet_Warehouse_Management_System.Tests.UnitTests.ProductsTests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _service = new ProductService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Name = "Prod1" },
                new Product { Name = "Prod2" }
            };
            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Prod1");
            result[1].Name.Should().Be("Prod2");
        }

        [Fact]
        public async Task GetByCodeAsync_ProductExists_ShouldReturnDto()
        {
            // Arrange
            var product = new Product { Name = "Prod1" };
            _repoMock.Setup(r => r.GetByCodeAsync("CODE1"))
                     .ReturnsAsync(product);

            // Act
            var result = await _service.GetByCodeAsync("CODE1");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Prod1");
        }

        [Fact]
        public async Task GetByCodeAsync_ProductNotExists_ShouldReturnNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByCodeAsync("UNKNOWN"))
                     .ReturnsAsync((Product?)null);

            // Act
            var result = await _service.GetByCodeAsync("UNKNOWN");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnDto()
        {
            // Arrange
            var dto = new ProductRequestDto { Name = "Nuovo" };
            var created = dto.ToProduct();
            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                     .ReturnsAsync(created);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Nuovo");
            _repoMock.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
