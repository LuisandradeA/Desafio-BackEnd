using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.DTOs.Request;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Services.Services;
using FluentValidation;
using Moq;

namespace DeliveryApp.UnitTests.Services
{
    public class MotorcycleServiceTests
    {
        private readonly Mock<IMotorcycleRepository> _repositoryMock;
        private readonly Mock<IValidator<CreateMotorcycleDTO>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateLicensePlateDTO>> _updateValidatorMock;
        private readonly MotorcycleService _service;

        public MotorcycleServiceTests()
        {
            _repositoryMock = new Mock<IMotorcycleRepository>();
            _createValidatorMock = new Mock<IValidator<CreateMotorcycleDTO>>();
            _updateValidatorMock = new Mock<IValidator<UpdateLicensePlateDTO>>();
            _service = new MotorcycleService(_repositoryMock.Object, _createValidatorMock.Object, _updateValidatorMock.Object);
        }

        [Fact]
        public async Task CreateMotorcycleAsync_ShouldReturnSuccess_WhenValidDTO()
        {
            // Arrange  
            var dto = new CreateMotorcycleDTO { Identifier = "123", Year = 2022, ModelName = "ModelX", LicensePlate = "ABC1234" };
            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Motorcycle>())).ReturnsAsync(true);

            // Act  
            var result = await _service.CreateMotorcycleAsync(dto);

            // Assert  
            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateMotorcycleAsync_ShouldReturnFailure_WhenInvalidDTO()
        {
            // Arrange  
            var dto = new CreateMotorcycleDTO();
            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult
            {
                Errors = { new FluentValidation.Results.ValidationFailure("Identifier", "Identifier is required") }
            });

            // Act  
            var result = await _service.CreateMotorcycleAsync(dto);

            // Assert  
            Assert.False(result.Success);
            Assert.Contains("Dados inválidos", result.Message);
        }

        [Fact]
        public async Task GetMotorcycleByIdAsync_ShouldReturnMotorcycle_WhenExists()
        {
            // Arrange  
            var id = "123";
            var motorcycle = new Motorcycle("123", 2022, "ModelX", "ABC1234");
            _repositoryMock.Setup(r => r.GetMotorcycleByIdAsync(id)).ReturnsAsync(motorcycle);

            // Act  
            var result = await _service.GetMotorcycleByIdAsync(id);

            // Assert  
            Assert.True(result.Success);
            Assert.NotNull(result.Result);
            Assert.Equal("123", result.Result.Identifier);
        }

        [Fact]
        public async Task GetMotorcycleByIdAsync_ShouldReturnFailure_WhenNotFound()
        {
            // Arrange  
            var id = "123";
            _repositoryMock.Setup(r => r.GetMotorcycleByIdAsync(id)).ReturnsAsync((Motorcycle)null);

            // Act  
            var result = await _service.GetMotorcycleByIdAsync(id);

            // Assert  
            Assert.False(result.Success);
            Assert.Equal("Moto não encontrada", result.Message);
        }

        [Fact]
        public async Task UpdateLicensePlateAsync_ShouldReturnSuccess_WhenValidDTO()
        {
            // Arrange  
            var dto = new UpdateLicensePlateDTO("123", "XYZ9876");
            var motorcycle = new Motorcycle("123", 2022, "ModelX", "ABC1234");
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _repositoryMock.Setup(r => r.GetMotorcycleByIdAsync(dto.Id)).ReturnsAsync(motorcycle);
            _repositoryMock.Setup(r => r.UpdateMotorcycleAsync(It.IsAny<Motorcycle>())).ReturnsAsync(true);

            // Act  
            var result = await _service.UpdateLicensePlateAsync(dto);

            // Assert  
            Assert.True(result.Success);
            Assert.Equal("Placa modificada com sucesso", result.Message);
        }

        [Fact]
        public async Task UpdateLicensePlateAsync_ShouldReturnFailure_WhenInvalidDTO()
        {
            // Arrange  
            var dto = new UpdateLicensePlateDTO("123", "");
            _updateValidatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult
            {
                Errors = { new FluentValidation.Results.ValidationFailure("LicensePlate", "License plate is required") }
            });

            // Act  
            var result = await _service.UpdateLicensePlateAsync(dto);

            // Assert  
            Assert.False(result.Success);
            Assert.Contains("License plate is required", result.Message);
        }

        [Fact]
        public async Task DeleteMotorcycleAsync_ShouldReturnSuccess_WhenMotorcycleExists()
        {
            // Arrange  
            var id = "123";
            var motorcycle = new Motorcycle("123", 2022, "ModelX", "ABC1234");
            _repositoryMock.Setup(r => r.GetMotorcycleByIdAsync(id)).ReturnsAsync(motorcycle);
            _repositoryMock.Setup(r => r.UpdateMotorcycleAsync(It.IsAny<Motorcycle>())).ReturnsAsync(true);

            // Act  
            var result = await _service.DeleteMotorcycleAsync(id);

            // Assert  
            Assert.True(result.Success);
            Assert.Equal("Moto deletada com sucesso", result.Message);
        }

        [Fact]
        public async Task DeleteMotorcycleAsync_ShouldReturnFailure_WhenMotorcycleNotFound()
        {
            // Arrange  
            var id = "123";
            _repositoryMock.Setup(r => r.GetMotorcycleByIdAsync(id)).ReturnsAsync((Motorcycle)null);

            // Act  
            var result = await _service.DeleteMotorcycleAsync(id);

            // Assert  
            Assert.False(result.Success);
            Assert.Equal("Dados inválidos", result.Message);
            _repositoryMock.Verify(r => r.UpdateMotorcycleAsync(It.IsAny<Motorcycle>()), Times.Never);
        }
    }
}
