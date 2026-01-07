namespace EnglishCenter.Extensions
{
    public static class FileUploadExtensions
    {
        private static readonly string[] AllowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public static async Task<string> SaveImageAsync(
            this IFormFile file,
            string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File rỗng");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException("Định dạng ảnh không hợp lệ");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("Ảnh vượt quá 5MB");

            var uploadFolder = Path.Combine("wwwroot", "uploads", folder);

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Trả về path public
            return $"/uploads/{folder}/{fileName}";
        }
    }
}
