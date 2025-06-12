using Microsoft.AspNetCore.Http;

namespace DeliveryApp.Application.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(string base64String);
    }
}
