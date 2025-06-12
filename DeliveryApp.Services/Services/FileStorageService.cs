using DeliveryApp.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DeliveryApp.Services.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly IConfiguration _configuration;

        public FileStorageService(
        IConfiguration configuration)
        {
            _configuration = configuration;
            _basePath = configuration["FileStorage:BasePath"] ?? "FileStorage";

            // Garante que o diretório base existe
            Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveFileAsync(string base64String)
        {
            // Validação da extensão
            //var extension = Path.GetExtension(fileName).ToLower();
            //if (extension != ".png" && extension != ".bmp")
            //{
            //    throw new InvalidOperationException("Somente arquivos PNG ou BMP são permitidos");
            //}

            // Remove o prefixo se existir (ex: "data:image/png;base64,")
            var base64Data = base64String.Contains(",")
                ? base64String.Split(',')[1]
                : base64String;

            // Cria diretório se não existir
            var fullPath = Path.Combine(_basePath, _configuration["FileStorage:DriverLicenseSubDir"] ?? "driver-licenses");
            Directory.CreateDirectory(fullPath);


            // Gera nome único
            //TODO: Nao deixar Png fixo aqui
            var uniqueFileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(fullPath, uniqueFileName);

            var bytes = Convert.FromBase64String(base64Data);
            await File.WriteAllBytesAsync(filePath, bytes);

            return filePath;
        }
    }
}
