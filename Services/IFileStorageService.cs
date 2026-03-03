namespace clipy.Services;

public interface IFileStorageService
{
    Task<string> UploadAsync(IFormFile file, string fileName);
    Task<Stream> GetFileStreamAsync(string fileName);
    Task DeleteAsync(string fileName);
}