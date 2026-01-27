using dotnet_Warehouse_Management_System.GoodsIn.Dtos;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace dotnet_Warehouse_Management_System.Tests.IntegrationTests
{
    public class ReceivingControllerTest(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();

        private GrnItemRequestDto CreateGrnItemRequestDto()
        {
            return new GrnItemRequestDto
            {
                ProductCode = "TestCode",
                ExpectedQty = 10,
                ReceivedQty = 10,
                CompliantQty = 10,
                NotCompliantQty = 0
            };
        }

        [Fact]
        public async Task GetGrnByCode_ReturnsGrn()
        {
            var grnCode = "GRN-001";
            var response = await _client.GetAsync($"/api/v1/receiving/grns/code/{grnCode}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var grn = await response.Content.ReadFromJsonWithEnumAsync<GrnResponseDto>();
            grn.Should().NotBeNull(); 
            grn.Code.Should().Be(grnCode);
        }

        [Fact]
        public async Task CreateGrnItem_ReturnsCreatedGrnItem()
        {
            var grnCode = "GRN-001";
            var newItem = CreateGrnItemRequestDto();
            var response = await _client.PostAsJsonAsync($"/api/v1/receiving/grns/{grnCode}/items", newItem);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var grnItem = await response.Content.ReadFromJsonWithEnumAsync<GrnItemResponseDto>();
            grnItem.Should().NotBeNull();
            grnItem.ProductCode.Should().Be(newItem.ProductCode);
            grnItem.ExpectedQty.Should().Be(newItem.ExpectedQty);
            grnItem.ReceivedQty.Should().Be(newItem.ReceivedQty);
            grnItem.CompliantQty.Should().Be(newItem.CompliantQty);
            grnItem.NotCompliantQty.Should().Be(newItem.NotCompliantQty);
        }







    }
}
