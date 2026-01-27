using dotnet_Warehouse_Management_System.Products.Entities.Services;
using dotnet_Warehouse_Management_System.Warehouses.Entities;
using dotnet_Warehouse_Management_System.Warehouses.Entities.Dtos;
using dotnet_Warehouse_Management_System.Warehouses.Entities.Mappers;
using dotnet_Warehouse_Management_System.Warehouses.Entities.Repositories;
using FluentAssertions;
using Moq;

namespace dotnet_Warehouse_Management_System.Tests.UnitTests.WarehousesTests
{
    public class SlotServiceTests
    {
        private readonly Mock<ISlotRepository> _slotRepository;
        private readonly SlotService _slotService;

        public SlotServiceTests()
        {
            _slotRepository = new Mock<ISlotRepository>();
            _slotService = new SlotService(_slotRepository.Object);
        }

        private Slot createSlot()
        {
            return new Slot{
                Code = "code",
                Category = Common.Category.STANDARD,
                PickingSequence = 1,
                Capacity = 1
            };
        }

        [Fact]
        public async Task GetTaskAsync_SlotExists_ShouldReturnSlotDto()
        {
            // Arrange
            var slot = createSlot();
            _slotRepository
                .Setup(x => x.GetByCodeAsync("code"))
                .ReturnsAsync(slot);

            // Act
            var result = await _slotService.GetByCodeAsync("code");
            // Assert
            result.Should().NotBeNull();
            result!.Code.Should().Be("code");
        }

        [Fact]
        public async Task GetTaskAsync_SlotNotExists_ShouldReturnNull()
        {
            // Arrange
            _slotRepository
                .Setup(x => x.GetByCodeAsync("code"))
                .ReturnsAsync((Slot?)null);

            // Act
            var result = await _slotService.GetByCodeAsync("code");
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateSlot()
        {
            var dto = new SlotRequestDto
            {
                Category = Common.Category.STANDARD,
                PickingSequence = 1,
                Capacity = 1
            };
            var slot = dto.ToSlot();
            _slotRepository
                .Setup(x => x.CreateAsync(It.IsAny<Slot>()))
                .ReturnsAsync(slot);
            var result = await _slotService.CreateAsync(dto);
            result.Should().NotBeNull();
            result!.Capacity.Should().Be(1);

        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteSlot()
        {
            var slot = createSlot();
            _slotRepository
                .Setup(x => x.DeleteAsync(slot.Code))
                .ReturnsAsync(slot);
            var result = await _slotService.DeleteAsync(slot.Code);

            result!.Code.Should().Be(slot.Code);
        }







    }
}
