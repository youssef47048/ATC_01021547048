using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Event_Management_System.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subDirectory = "events")
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Create directory path if it doesn't exist
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", subDirectory);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Create unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return relative path for storage in database
            return $"/images/{subDirectory}/{uniqueFileName}";
        }

        public void DeleteFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                // Convert relative path to full path
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, path.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception)
            {
                // Log the error but continue
            }
        }
    }
} 