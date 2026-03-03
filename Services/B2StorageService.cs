using Amazon.S3;
using Amazon.S3.Model;

namespace clipy.Services;

public class B2StorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3;
    private readonly IConfiguration _config;
    private readonly string _bucket;

    public B2StorageService(IConfiguration config)
    {
        _config = config;
        _bucket = _config["B2:BucketName"]!;

        var s3Config = new AmazonS3Config
        {
            ServiceURL = _config["B2:ServiceUrl"],
            ForcePathStyle = true
        };

        _s3 = new AmazonS3Client(
            _config["B2:KeyId"],
            _config["B2:ApplicationKey"],
            s3Config
        );
    }

    public async Task<string> UploadAsync(IFormFile file, string fileName)
    {
        using var stream = file.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = fileName,
            InputStream = stream,
            ContentType = file.ContentType
        };

        await _s3.PutObjectAsync(request);

        return fileName;
    }
    public async Task<Stream> GetFileStreamAsync(string fileName)
    {
        var response = await _s3.GetObjectAsync(_bucket, fileName);
        Console.WriteLine("fileaccedds ", response.ToString());

        var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }

    public async Task DeleteAsync(string fileName)
    {
        await _s3.DeleteObjectAsync(_bucket, fileName);
    }

}