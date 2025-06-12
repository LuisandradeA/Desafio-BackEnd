using DeliveryApp.Application.DTOs.Request.Driver;
using DeliveryApp.Application.DTOs.Response;
using DeliveryApp.Application.Services;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using FluentValidation;

namespace DeliveryApp.Services.Services
{
    public class DriverService : IDriverService
    {
        private readonly IValidator<CreateDriverDTO> _createDriverValidator;
        private readonly IDriverRepository _repository;
        private readonly IFileStorageService _fileStorageService;

        public DriverService(
            IValidator<CreateDriverDTO> createDriverValidator,
            IDriverRepository repository,
            IFileStorageService fileStorageService)
        {
            _createDriverValidator = createDriverValidator;
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Response<DefaultResult>> CreateDriverAsync(CreateDriverDTO dto)
        {
            try
            {
                var validationResult = await _createDriverValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    return new Response<DefaultResult>(false, "Dados inválidos");
                }

                var driver = new Driver(
                    dto.Identifier,
                    dto.Name,
                    dto.Document,
                    dto.BirthDate,
                    dto.LicenseNumber,
                    dto.LicenseType);

                var result = await _repository.CreateAsync(driver);
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

        public async Task<Response<DefaultResult>> UpdateLicensePlateImageAsync(string id, string base64String)
        {
            var driver = await _repository.GetByIdAsync(id);

            if(driver is null)
            {
                return new Response<DefaultResult>(false, "Motorista não encontrado.");
            }

            var filePath = await _fileStorageService.SaveFileAsync(base64String);

            if (string.IsNullOrEmpty(filePath))
            {
                return new Response<DefaultResult>(false, "Erro ao salvar a imagem.");
            }

            driver.SetLicenseImagePath(filePath);
            await _repository.UpdateAsync(driver);

            return new Response<DefaultResult>(true, "Imagem de placa atualizada com sucesso");
        }
    }
}
