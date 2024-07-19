using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Files.Commands
{
    public record UploadFile(IFormFile File) : IRequest<FileResponseDto>;

    public class UploadFileHandler : IRequestHandler<UploadFile, FileResponseDto>
    {
        private readonly IFileService _fileService;
        private readonly ILogger<UploadFileHandler> _logger;

        public UploadFileHandler(IFileService fileService, ILogger<UploadFileHandler> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<FileResponseDto> Handle(UploadFile request, CancellationToken ct)
        {
            using var stream = request.File.OpenReadStream();
            var blobName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            var result = await _fileService.UploadAsync(stream, blobName, ct);

            _logger.LogInformation($"Uploaded new file on blob storage");
            return new FileResponseDto
            {
                Uri = result
            };
        }
    }
}
