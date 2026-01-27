using dotnet_Warehouse_Management_System.Outbound.Dtos;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace dotnet_Warehouse_Management_System.Tests.IntegrationTests
{
    public class PicklistGenerationTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task GeneratePicklists_ReturnsCreatedPicklists()
        {
            // Arrange
            var orderIds = new List<long> { 1 };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/picklists/release", orderIds);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var picklists = await response.Content.ReadFromJsonWithEnumAsync<List<PicklistDto>>();

            picklists.Should().NotBeNull();
            picklists.Should().HaveCount(1);

            var picklist = picklists.First();
            picklist.CustomerCode.Should().Be("CUST-001");
            picklist.ReleaseNumber.Should().StartWith("PKL-");
            picklist.pickListItemList.Should().HaveCount(1);

            var item = picklist.pickListItemList.First();
            item.productCode.Should().Be("PROD-001");
            item.salesOrderCode.Should().Be("ORD-001");
            item.State.Should().Be(Common.PicklistItemState.OPEN);
        }








    }
}
