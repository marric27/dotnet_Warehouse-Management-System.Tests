using dotnet_Warehouse_Management_System.Outbound.Dtos;
using dotnet_Warehouse_Management_System.Outbound.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace dotnet_Warehouse_Management_System.Tests.IntegrationTests
{
    public class SalesOrderControllerTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task CreateOrder_ReturnsCreatedOrder()
        {
            // Arrange
            var request = new OrderRequestDto
            {
                CustomerCode = "CUST-001",
                SalesOrderLineList = [
                    new SalesOrderLineRequestDto
                    {
                        productCode = "PROD-001",
                        quantity = 5
                    }
                ]
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/sales-order/create-order", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var order = await response.Content.ReadFromJsonWithEnumAsync<OrderResponseDto>();

            order.Should().NotBeNull();
            order.customerCode.Should().Be("CUST-001");
            order.state.Should().Be(OrderState.OPEN);

            order.salesOrderLineList.Should().HaveCount(1);
            order.salesOrderLineList[0].productCode.Should().Be("PROD-001");
            order.salesOrderLineList[0].quantity.Should().Be(5);
        }
    }
}