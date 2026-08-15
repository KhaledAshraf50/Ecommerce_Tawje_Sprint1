namespace ECommerce_Tawj.Services.FilesService
{
    public interface IFileService
    {
       Task<List<string>> UploadFile(List<IFormFile> files);

        bool DeleteFile(string? filePath);

    }
}
