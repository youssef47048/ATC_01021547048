using Microsoft.AspNetCore.Http;

namespace Event_Management_System.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string subDirectory = "events");
        void DeleteFile(string path);
    }
} 