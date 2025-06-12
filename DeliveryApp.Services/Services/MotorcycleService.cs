using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.DTOs.Request;
using DeliveryApp.Application.DTOs.Response;
using DeliveryApp.Application.DTOs.Response.Motorcycle;
using DeliveryApp.Application.Services;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using FluentValidation;

namespace DeliveryApp.Services.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        //TODO:
        //private readonly ILogger<MotorcycleService> _logger;
        private readonly IMotorcycleRepository _repository;
        private readonly IValidator<CreateMotorcycleDTO> _createMotorcycleDTOValidator;
        private readonly IValidator<UpdateLicensePlateDTO> _updateLicensePlateValidator;

        public MotorcycleService(
            IMotorcycleRepository repository,
            IValidator<CreateMotorcycleDTO> createMotorcycleDTOValidator,
            IValidator<UpdateLicensePlateDTO> updateLicensePlateValidator
            )
        {
            _repository = repository;
            _createMotorcycleDTOValidator = createMotorcycleDTOValidator;
            _updateLicensePlateValidator = updateLicensePlateValidator;
        }

        public async Task<Response<DefaultResult>> CreateMotorcycleAsync(CreateMotorcycleDTO dto)
        {
            try
            {
                //Validate the DTO
                var validationResult = await _createMotorcycleDTOValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return new Response<DefaultResult>(false, "Dados inválidos");
                }

                //Map DTO to Entity
                var motorcycle = new Motorcycle(
                    dto.Identifier,
                    dto.Year,
                    dto.ModelName,
                    dto.LicensePlate);

                //Save the entity to the repository
                var result = await _repository.CreateAsync(motorcycle);
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

        public async Task<Response<DefaultResult>> DeleteMotorcycleAsync(string id)
        {
            try
            {
                var motorcycle = await _repository.GetMotorcycleByIdAsync(id);

                if (motorcycle is null)
                {
                    return new Response<DefaultResult>(false, "Dados inválidos")
                    {
                        IsBadRequest = true
                    };
                }

                //TODO: Check Rental exists

                motorcycle.DeleteMotorcycle();
                var result = await _repository.UpdateMotorcycleAsync(motorcycle);
                return new Response<DefaultResult>(true, "Moto deletada com sucesso")
                {
                    Result = new DefaultResult()
                };
            }
            catch (Exception ex)
            {
                return new Response<DefaultResult>(false, ex.Message)
                {
                    IsServerError = true
                };
            }
        }

        public async Task<Response<GetMotorcyclesResponseDTO>> GetMotorcycleByIdAsync(string id)
        {
            try
            {
                if(string.IsNullOrEmpty(id))
                {
                    return new Response<GetMotorcyclesResponseDTO>(false, "Request mal formada")
                    {
                        IsBadRequest = true
                    };
                }

                var result = await _repository.GetMotorcycleByIdAsync(id);

                if (result is null)
                {
                    return new Response<GetMotorcyclesResponseDTO>(false, "Moto não encontrada");
                }

                var response = new GetMotorcyclesResponseDTO
                {
                    Identifier = result.Identifier,
                    Year = result.Year,
                    ModelName = result.ModelName,
                    LicensePlate = result.LicensePlate
                };

                return new Response<GetMotorcyclesResponseDTO>(true)
                {
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new Response<GetMotorcyclesResponseDTO>(false, ex.Message)
                {
                    IsServerError = true
                };
            }
        }

        public async Task<Response<List<GetMotorcyclesResponseDTO>>> GetMotorcyclesAsync()
        {
            try
            {
                var result = await _repository.GetMotorcyclesAsync();

                if (result is not null && result.Any())
                {
                    var response = result.Select(m => new GetMotorcyclesResponseDTO
                    {
                        Identifier = m.Identifier,
                        Year = m.Year,
                        ModelName = m.ModelName,
                        LicensePlate = m.LicensePlate
                    }).ToList();

                    return new Response<List<GetMotorcyclesResponseDTO>>(true, "Motorcycles retrieved successfully")
                    {
                        Result = response
                    };
                }

                return new Response<List<GetMotorcyclesResponseDTO>>(false, "No motorcycles found")
                {
                    IsServerError = false
                };
            }
            catch (Exception ex)
            {
                return new Response<List<GetMotorcyclesResponseDTO>>(false, ex.Message)
                {
                    IsServerError = true
                };
            }
        }

        public async Task<Response<DefaultResult>> UpdateLicensePlateAsync(UpdateLicensePlateDTO dto)
        {
            try
            {
                var validationResult = await _updateLicensePlateValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return new Response<DefaultResult>(false, string.Join(";", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                var motorcycle = await _repository.GetMotorcycleByIdAsync(dto.Id);
                if (motorcycle == null)
                {
                    return new Response<DefaultResult>(false, "Dados inválidos");
                }

                motorcycle.SetLicensePlate(dto.LicensePlate);
                var updateResult = await _repository.UpdateMotorcycleAsync(motorcycle);

                if (updateResult)
                {
                    return new Response<DefaultResult>(true, "Placa modificada com sucesso");
                }
                return new Response<DefaultResult>(false, "Failed to update license plate");
            }
            catch (Exception ex)
            {
                return new Response<DefaultResult>(false, ex.Message)
                {
                    IsServerError = true
                };
            }
        }
    }
}
