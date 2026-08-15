namespace ECommerce_Tawj.Services.FilesService
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string[] _allowedExtenstions = {".jpg", ".jpeg", ".png", ".webp"};  

        private const long MaxFileSizeInBytes = 2 * 1024 * 1024; // 2 MB
        public LocalFileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<List<string>> UploadFile(List<IFormFile> files)
        {
            if(files == null || files.Count == 0) return  new List<string> { "/uploads/products/default.png" };
            if(files.Count > 4)
            {
                throw new Exception("You can upload a maximum of 4 files.");
            }
            var uploadedFilePaths = new List<string>();
            // Validate first
            foreach (var file in files)
            {
                if (file.Length > MaxFileSizeInBytes)
                {
                    throw new Exception("File size exceeds the maximum limit of 2 MB.");
                }
                var FileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtenstions.Contains(FileExtension))
                {
                    throw new Exception("Invalid file type. Only .jpg, .jpeg, .png, and .webp files are allowed.");
                }
            }
            // Upload after validation
            var pathFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads","products");

            if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);
            foreach (var file in files)
            { 
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var FilePath = Path.Combine(pathFolder, fileName);
                using var stream = new FileStream(FilePath, FileMode.Create);
                await file.CopyToAsync(stream);

                uploadedFilePaths.Add("/uploads/products/" + fileName);

            }
            return uploadedFilePaths;
        }
        public bool DeleteFile(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || filePath == "/uploads/products/default.png") return false;
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/','\\'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
    }
}
