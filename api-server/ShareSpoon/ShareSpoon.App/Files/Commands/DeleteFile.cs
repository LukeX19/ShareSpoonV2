using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;

namespace ShareSpoon.App.Files.Commands
{
    public record DeleteFile(string BlobName) : IRequest<Unit>;

    public class DeleteFileHandler : IRequestHandler<DeleteFile, Unit>
    {
        private readonly IFileService _fileService;
        private readonly ILogger<DeleteFileHandler> _logger;

        public DeleteFileHandler(IFileService fileService, ILogger<DeleteFileHandler> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteFile request, CancellationToken ct)
        {
            await _fileService.DeleteAsync(request.BlobName, ct);

            _logger.LogInformation($"Deleted a file from blob storage");
            return Unit.Value;
        }
    }
}
