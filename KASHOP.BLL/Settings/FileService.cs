using Microsoft.AspNetCore.Http;

namespace KASHOP.BLL.Settings
{
    public class FileService : IFileService
    {
        private static readonly string[] AllowedExtentions = { ".jpg", ".png", ".webp" };
        const long MaxFileSize = 5 * 1024 * 1024; // 5MB;
        private static readonly string ImagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public async Task<string?> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file is null || file.Length == 0 || file.Length > MaxFileSize) return null;
            var extention = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtentions.Contains(extention)) return null;
            var fileName = Guid.NewGuid().ToString() + extention;
            Directory.CreateDirectory(ImagesPath);
            var filePath = Path.Combine(ImagesPath, fileName);
            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream, cancellationToken);
            return fileName;
        }

        public void Delete(string FileName)
        {
            var filePath = Path.Combine(ImagesPath, FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}