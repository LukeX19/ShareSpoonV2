using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using ShareSpoon.App.Abstractions;
using ShareSpoon.Infrastructure.Exceptions;
using ShareSpoon.Infrastructure.Options;

namespace ShareSpoon.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly BlobStorageSettings _settings;
        private readonly BlobContainerClient _containerClient;
        private readonly List<string> _supportedExtensions = new List<string> { "png", "jpg", "jpeg" };

        public FileService(IOptions<BlobStorageSettings> settings)
        {
            _settings = settings.Value;
            _containerClient = new BlobContainerClient(_settings.ConnectionString, _settings.ContainerName);
        }

        public async Task<string> UploadAsync(Stream stream, string blobName, CancellationToken ct = default)
        {
            var fileExtension = blobName.Split(".")[1];
            if (!_supportedExtensions.Contains(fileExtension))
            {
                throw new InvalidImageFormatException();
            }

            await EnsureContainerExists();
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(stream, ct);

            return GetContentUrl(blobName);
        }

        public async Task DeleteAsync(string blobName, CancellationToken ct = default)
        {
            var splitBlobName = blobName.Split(_settings.ContainerName + "/")[1];

            var blobClient = _containerClient.GetBlobClient(splitBlobName);
            var existingBlob = await blobClient.ExistsAsync(ct);
            if (!existingBlob.Value)
            {
                throw new BlobNotFoundException(splitBlobName);
            }

            await blobClient.DeleteAsync(cancellationToken: ct);
        }

        private async Task EnsureContainerExists()
        {
            var result = await _containerClient.ExistsAsync();

            if (!result.Value)
            {
                await _containerClient.CreateAsync();
            }
        }

        private string GetContentUrl(string blobName)
        {
            return string.Format(_settings.BlobURLFormat, _settings.ContainerName, blobName);
        }
    }
}
