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
        public async Task DeleteAsync_ShouldReturnTrue_WhenSlotExistsAndIsDeleted()
        {
            // Arrange
            var slot = createSlot(); // Immagino restituisca un oggetto con Code popolato
            var slotCode = slot.Code;

            // Setup: il service prima controlla se esiste
            _slotRepository
                .Setup(x => x.GetByCodeAsync(slotCode))
                .ReturnsAsync(slot);

            // Setup: esecuzione della cancellazione
            _slotRepository
                .Setup(x => x.DeleteAsync(slotCode))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _slotService.DeleteAsync(slotCode);

            // Assert
            result.Should().BeTrue(); // Il service restituisce bool

            // Verifica che il repository sia stato chiamato esattamente una volta con il codice corretto
            _slotRepository.Verify(x => x.DeleteAsync(slotCode), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenSlotDoesNotExist()
        {
            // Arrange
            string nonExistentCode = "NON-EXISTENT";

            _slotRepository
                .Setup(x => x.GetByCodeAsync(nonExistentCode))
                .ReturnsAsync((Slot)null); // Simula slot non trovato

            // Act
            var result = await _slotService.DeleteAsync(nonExistentCode);

            // Assert
            result.Should().BeFalse();

            // Verifica che DeleteAsync NON sia mai stato chiamato se lo slot non esiste
            _slotRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
        }







    }
}
