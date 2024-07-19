namespace ShareSpoon.App.Abstractions
{
    public interface IFileService
    {
        Task<string> UploadAsync(Stream stream, string blobName, CancellationToken ct = default);
        Task DeleteAsync(string blobName, CancellationToken ct = default);
    }
}
