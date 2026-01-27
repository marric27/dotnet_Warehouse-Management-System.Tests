using dotnet_Warehouse_Management_System.Products.Entities.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dotnet_Warehouse_Management_System.Tests.IntegrationTests
{
    public class ProductControllerTest(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();

        private ProductRequestDto CreateProductRequestDto() => new() { Name = "Test Product", Category = Common.Category.STANDARD };


        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/products");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = CreateProductRequestDto();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            // Act
            var createResponse = await _client.PostAsJsonAsync("/api/v1/products", newProduct);

            // Assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponseDto>(options);
            createdProduct.Should().NotBeNull();
            createdProduct.Name.Should().Be("Test Product");
            createdProduct.Category.Should().Be(Common.Category.STANDARD);
            createdProduct.Code.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetByCode_ReturnsProduct()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            // Act: GET by code
            var getResponse = await _client.GetAsync($"/api/v1/products/code/TestCode");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var prod = await getResponse.Content.ReadFromJsonWithEnumAsync<ProductResponseDto>();
            prod.Should().NotBeNull();
            prod.Name.Should().Be("Test Product");
            prod.Category.Should().Be(Common.Category.STANDARD);
            prod.Code.Should().Be("TestCode");
        }






    }
}
