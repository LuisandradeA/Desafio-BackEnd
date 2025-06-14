using DeliveryApp.Application.DTOs.Request.Rentals;
using DeliveryApp.Application.DTOs.Response;
using DeliveryApp.Application.DTOs.Response.Rental;
using DeliveryApp.Application.Services;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using FluentValidation;

namespace DeliveryApp.Services.Services
{
    public class RentalService : IRentalService
    {
        private readonly IValidator<CreateRentalDTO> _createRentalValidator;
        private readonly IRentalRepository _repository;
        private readonly IDriverRepository _driverRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public RentalService(
            IValidator<CreateRentalDTO> createRentalValidator,
            IRentalRepository repository,
            IDriverRepository driverRepository,
            IMotorcycleRepository motorcycleRepository)
        {
            _createRentalValidator = createRentalValidator;
            _repository = repository;
            _driverRepository = driverRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<Response<DefaultResult>> CreateRentalAsync(CreateRentalDTO dto)
        {
            try
            {
                var validationResult = await _createRentalValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return new Response<DefaultResult>(false, "Dados inválidos");
                }

                if (!await IsDriverAvailableAsync(dto.DriverId) &&
                    !await IsMotorcycleAvailableAsync(dto.MotorcycleId))
                {
                    return new Response<DefaultResult>(false, "Motorista não disponível");
                }

                var startDate = GetNextDayAtMidnight(dto.StartTime);

                var rental = new Rentals(
                    Guid.NewGuid().ToString(),
                    dto.PlanType,
                    startDate,
                    dto.EndTime,
                    dto.PrevisionTime,
                    dto.MotorcycleId,
                    dto.DriverId,
                    CalculateInitialPrice(dto.PlanType));

                var result = await _repository.CreateAsync(rental);
                return new Response<DefaultResult>(true);
            }
            catch (Exception ex)
            {
                return new Response<DefaultResult>(false, ex.Message)
                {
                    IsServerError = true
                };
            }
        }

        private decimal CalculateInitialPrice(int planType)
        {
            return planType switch
            {
                7 => 7 * 30.00m,
                15 => 15 * 28.00m,
                30 => 30 * 22.00m,
                45 => 45 * 20.00m,
                50 => 50 * 18.00m,
                _ => 0
            };
        }

        private async Task<bool> IsDriverAvailableAsync(string driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            return driver != null &&
                   (driver.Rentals == null || !driver.Rentals.Any(x => x.IsActive)) &&
                   driver.LicenseType == "A";
        }

        private async Task<bool> IsMotorcycleAvailableAsync(string motorcycleId)
        {
            var motorcycle = await _motorcycleRepository.GetMotorcycleByIdAsync(motorcycleId);
            return motorcycle != null && 
                (motorcycle.Rentals == null || !motorcycle.Rentals.Any(x => x.IsActive));
        }

        private static DateTime GetNextDayAtMidnight(DateTime date)
        {
            return date.Date.AddDays(1);
        }

        public async Task<Response<GetRentalResponseDTO>> GetRentalByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return new Response<GetRentalResponseDTO>(false, "Request mal formada")
                    {
                        IsBadRequest = true
                    };
                }

                var result = await _repository.GetRentalByIdAsync(id);

                if (result is null)
                {
                    return new Response<GetRentalResponseDTO>(false, "Moto não encontrada");
                }

                var response = new GetRentalResponseDTO
                {
                    Identifier = result.Identifier,
                    DayliPrice = GetPlanPrice(result.PlanType),
                    DriverId = result.DriverId,
                    MotorcycleId = result.MotorcycleId,
                    StartTime = result.StartTime,
                    EndTime = result.EndTime,
                    PrevisionTime = result.PrevisionTime,
                    ReturnDate = result.EndTime
                };

                return new Response<GetRentalResponseDTO>(true)
                {
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new Response<GetRentalResponseDTO>(false, ex.Message)
                {
                    IsServerError = true
                };
            }
        }

        private decimal GetPlanPrice(int planType)
        {
            return planType switch
            {
                7 => 30.00m,
                15 => 28.00m,
                30 => 22.00m,
                45 => 20.00m,
                50 => 18.00m,
                _ => 0
            };
        }

        public async Task<Response<DefaultResult>> ReturnRentalAsync(string id, DateTime returnDate)
        {
            try
            {
                var rental = await _repository.GetRentalByIdAsync(id);

                if (!IsRentalReturnValid(id, returnDate, rental))
                {
                    return new Response<DefaultResult>(false, "Dados inválidos")
                    {
                        IsBadRequest = true
                    };
                }

                if(returnDate < rental.PrevisionTime)
                {
                    RenturnRentalBeforeEnds(rental, returnDate);
                }
                else if (returnDate > rental.PrevisionTime)
                {
                    ReturnRentalAfterEnds(rental, returnDate);
                }
                else
                {
                    // Caso a devolução seja exatamente na data prevista
                    rental.ReturnRental(returnDate);
                }
                
                await _repository.UpdateAsync(rental);
                return new Response<DefaultResult>(true, "Data de devolução informada com sucesso");
            }
            catch (Exception ex)
            {
                return new Response<DefaultResult>(false, ex.Message)
                {
                    IsServerError = true
                };
            }
        }

        private void ReturnRentalAfterEnds(Rentals rental, DateTime returnDate)
        {
            // Dias adicionais
            var extraDays = (returnDate - rental.PrevisionTime).TotalDays;

            const decimal dailyAdditionalCharge = 50.00m;

            var additionalCharge = dailyAdditionalCharge * (decimal)extraDays;

            rental.SetAdditionalFee(additionalCharge);
            rental.ReturnRental(returnDate);
        }

        private void RenturnRentalBeforeEnds(Rentals rental, DateTime returnDate)
        {
            // Dias não utilizados
            var unusedDays = Math.Floor((rental.PrevisionTime - returnDate).TotalDays);

            var unusedDaysValue = GetPlanPrice(rental.PlanType) * (decimal)unusedDays;

            decimal fine = 0;
            if (rental.PlanType == 7)
            {
                fine = unusedDaysValue * 0.2m; // 20% de multa
            }
            else if (rental.PlanType == 15)
            {
                fine = unusedDaysValue * 0.4m; // 40% de multa
            }

            rental.Price = rental.Price - unusedDaysValue;
            rental.SetAdditionalFee(fine);
            rental.ReturnRental(returnDate);
        }

        private bool IsRentalReturnValid(string id, DateTime returnDate, Rentals rental)
        {
            if (rental is null)
            {
                return false;
            }

            if (returnDate <= rental.StartTime)
            {
                return false;
            }

            return true;
        }
    }
}
