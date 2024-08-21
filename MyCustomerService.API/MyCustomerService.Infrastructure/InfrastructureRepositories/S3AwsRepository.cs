using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using MyCustomerService.Infrastructure.InfrastructureRepositories.Interfaces;

namespace MyCustomerService.Infrastructure.InfrastructureRepositories;

public class S3AwsRepository : IFileRepository
{
  private readonly IAmazonS3 _s3Client;
  private readonly string _bucketName;

  public S3AwsRepository(IAmazonS3 s3Client, string bucketName)
  {
    _s3Client = s3Client;
    _bucketName = bucketName;
  }

  public async Task DeleteFileAsync(string filePath)
  {
    var deleteRequest = new DeleteObjectRequest
    {
      BucketName = _bucketName,
      Key = filePath
    };

    await _s3Client.DeleteObjectAsync(deleteRequest);
  }

  public async Task<Stream> DownloadFileAsync(string filePath)
  {
    var downloadRequest = new GetObjectRequest
    {
      BucketName = _bucketName,
      Key = filePath
    };

    var response = await _s3Client.GetObjectAsync(downloadRequest);
    return response.ResponseStream;
  }

  public async Task<IEnumerable<string>> ListFileAsync(string directoryPath)
  {
    var listRequest = new ListObjectsV2Request
    {
      BucketName = _bucketName,
      Prefix = directoryPath
    };

    var listResponse = await _s3Client.ListObjectsV2Async(listRequest);
    return listResponse.S3Objects.Select(o => o.Key).ToList();
  }

  public async Task<string> UploadFileAsync(IFormFile file, string destinationPath)
  {
    using (var stream = file.OpenReadStream())
    {
      var uploadRequest = new TransferUtilityUploadRequest
      {
        InputStream = stream,
        Key = destinationPath,
        BucketName = _bucketName,
        ContentType = file.ContentType
      };

      var fileTransferUtility = new TransferUtility(_s3Client);
      await fileTransferUtility.UploadAsync(uploadRequest);
    }

    return $"https://{_bucketName}.s3.amazonaws.com/{destinationPath}";
  }
}
