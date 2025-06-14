using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryApp.Application.DTOs.Request.Rentals;
using DeliveryApp.Application.DTOs.Response;
using DeliveryApp.Application.DTOs.Response.Rental;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Services.Services;
using FluentValidation;
using Moq;
using Xunit;

namespace DeliveryApp.UnitTests.Services
{
    public class RentalServiceTests
    {
        private readonly Mock<IValidator<CreateRentalDTO>> _mockValidator;
        private readonly Mock<IRentalRepository> _mockRentalRepository;
        private readonly Mock<IDriverRepository> _mockDriverRepository;
        private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
        private readonly RentalService _rentalService;

        public RentalServiceTests()
        {
            _mockValidator = new Mock<IValidator<CreateRentalDTO>>();
            _mockRentalRepository = new Mock<IRentalRepository>();
            _mockDriverRepository = new Mock<IDriverRepository>();
            _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();

            _rentalService = new RentalService(
                _mockValidator.Object,
                _mockRentalRepository.Object,
                _mockDriverRepository.Object,
                _mockMotorcycleRepository.Object);
        }

        [Fact]
        public async Task CreateRentalAsync_ShouldReturnSuccess_WhenDataIsValid()
        {
            // Arrange
            var dto = new CreateRentalDTO
            {
                DriverId = "driver1",
                MotorcycleId = "motorcycle1",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(7),
                PrevisionTime = DateTime.Now.AddDays(7),
                PlanType = 7
            };

            _mockValidator.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockDriverRepository.Setup(r => r.GetByIdAsync(dto.DriverId))
                .ReturnsAsync(new Driver("driver1", "John Doe", "123456789", DateTime.Now.AddYears(-30), "12345", "A"));

            _mockMotorcycleRepository.Setup(r => r.GetMotorcycleByIdAsync(dto.MotorcycleId))
                .ReturnsAsync(new Motorcycle("motorcycle1", 2020, "Model X", "ABC1234"));

            _mockRentalRepository.Setup(r => r.CreateAsync(It.IsAny<Rentals>()))
                .ReturnsAsync(true);

            // Act
            var result = await _rentalService.CreateRentalAsync(dto);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetRentalByIdAsync_ShouldReturnRental_WhenRentalExists()
        {
            // Arrange
            var rentalId = "rental1";
            var rental = new Rentals(
                rentalId,
                7,
                DateTime.Now,
                DateTime.Now.AddDays(7),
                DateTime.Now.AddDays(7),
                "motorcycle1",
                "driver1",
                210.00m);

            _mockRentalRepository.Setup(r => r.GetRentalByIdAsync(rentalId))
                .ReturnsAsync(rental);

            // Act
            var result = await _rentalService.GetRentalByIdAsync(rentalId);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Result);
            Assert.Equal(rentalId, result.Result.Identifier);
        }

        [Fact]
        public async Task ReturnRentalAsync_ShouldReturnSuccess_WhenReturnIsValid()
        {
            // Arrange
            var rentalId = "rental1";
            var returnDate = DateTime.Now.AddDays(8);
            var rental = new Rentals(
                rentalId,
                7,
                DateTime.Now,
                DateTime.Now.AddDays(7),
                DateTime.Now.AddDays(7),
                "motorcycle1",
                "driver1",
                210.00m);

            _mockRentalRepository.Setup(r => r.GetRentalByIdAsync(rentalId))
                .ReturnsAsync(rental);

            _mockRentalRepository.Setup(r => r.UpdateAsync(It.IsAny<Rentals>()))
                .ReturnsAsync(true);

            // Act
            var result = await _rentalService.ReturnRentalAsync(rentalId, returnDate);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task ReturnRentalAsync_ShouldReturnBadRequest_WhenReturnDateIsInvalid()
        {
            // Arrange
            var rentalId = "rental1";
            var returnDate = DateTime.Now.AddDays(-1);
            var rental = new Rentals(
                rentalId,
                7,
                DateTime.Now,
                DateTime.Now.AddDays(7),
                DateTime.Now.AddDays(7),
                "motorcycle1",
                "driver1",
                210.00m);

            _mockRentalRepository.Setup(r => r.GetRentalByIdAsync(rentalId))
                .ReturnsAsync(rental);

            // Act
            var result = await _rentalService.ReturnRentalAsync(rentalId, returnDate);

            // Assert
            Assert.False(result.Success);
            Assert.True(result.IsBadRequest);
        }
    }
}
