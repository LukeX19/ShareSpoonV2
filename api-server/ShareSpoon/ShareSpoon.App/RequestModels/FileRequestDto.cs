using Microsoft.AspNetCore.Http;

namespace ShareSpoon.App.RequestModels
{
    public class FileRequestDto
    {
        public IFormFile File { get; set; }
    }
}
