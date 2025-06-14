using System;
using System.Threading.Tasks;
using DeliveryApp.Application.DTOs.Request.Driver;
using DeliveryApp.Application.DTOs.Response;
using DeliveryApp.Application.Services;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Services.Services;
using FluentValidation;
using Moq;
using Xunit;

namespace DeliveryApp.UnitTests.Services
{
    public class DriverServiceTests
    {
        private readonly Mock<IValidator<CreateDriverDTO>> _mockValidator;
        private readonly Mock<IDriverRepository> _mockRepository;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly DriverService _driverService;

        public DriverServiceTests()
        {
            _mockValidator = new Mock<IValidator<CreateDriverDTO>>();
            _mockRepository = new Mock<IDriverRepository>();
            _mockFileStorageService = new Mock<IFileStorageService>();
            _driverService = new DriverService(_mockValidator.Object, _mockRepository.Object, _mockFileStorageService.Object);
        }

        [Fact]
        public async Task CreateDriverAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange  
            var dto = new CreateDriverDTO();
            _mockValidator.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = { new FluentValidation.Results.ValidationFailure("Identifier", "Identifier is required") }
                });

            // Act  
            var result = await _driverService.CreateDriverAsync(dto);

            // Assert  
            Assert.False(result.Success);
            Assert.Equal("Dados inválidos", result.Message);
        }

        [Fact]
        public async Task CreateDriverAsync_ShouldReturnSuccess_WhenDriverIsCreated()
        {
            // Arrange  
            var dto = new CreateDriverDTO
            {
                Identifier = "123",
                Name = "John Doe",
                Document = "123456789",
                BirthDate = DateTime.Now.AddYears(-30),
                LicenseNumber = "ABC123",
                LicenseType = "B"
            };

            _mockValidator.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Driver>()))
                .ReturnsAsync(true);

            // Act  
            var result = await _driverService.CreateDriverAsync(dto);

            // Assert  
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateLicensePlateImageAsync_ShouldReturnError_WhenDriverNotFound()
        {
            // Arrange  
            var id = "123";
            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Driver)null);

            // Act  
            var result = await _driverService.UpdateLicensePlateImageAsync(id, "base64string");

            // Assert  
            Assert.False(result.Success);
            Assert.Equal("Motorista não encontrado.", result.Message);
        }

        [Fact]
        public async Task UpdateLicensePlateImageAsync_ShouldReturnError_WhenFileSaveFails()
        {
            // Arrange  
            var id = "123";
            var driver = new Driver("123", "John Doe", "123456789", DateTime.Now.AddYears(-30), "ABC123", "B");
            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(driver);
            _mockFileStorageService.Setup(f => f.SaveFileAsync(It.IsAny<string>()))
                .ReturnsAsync(string.Empty);

            // Act  
            var result = await _driverService.UpdateLicensePlateImageAsync(id, "base64string");

            // Assert  
            Assert.False(result.Success);
            Assert.Equal("Erro ao salvar a imagem.", result.Message);
        }

        [Fact]
        public async Task UpdateLicensePlateImageAsync_ShouldReturnSuccess_WhenImageIsUpdated()
        {
            // Arrange  
            var id = "123";
            var driver = new Driver("123", "John Doe", "123456789", DateTime.Now.AddYears(-30), "ABC123", "B");
            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(driver);
            _mockFileStorageService.Setup(f => f.SaveFileAsync(It.IsAny<string>()))
                .ReturnsAsync("path/to/image.jpg");
            _mockRepository.Setup(r => r.UpdateAsync(driver))
                .ReturnsAsync(true);

            // Act  
            var result = await _driverService.UpdateLicensePlateImageAsync(id, "base64string");

            // Assert  
            Assert.True(result.Success);
            Assert.Equal("Imagem de placa atualizada com sucesso", result.Message);
        }
    }
}
