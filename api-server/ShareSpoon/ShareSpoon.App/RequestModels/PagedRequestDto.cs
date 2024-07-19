using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class PagedRequestDto
    {
        [Required]
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
