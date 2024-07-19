using ShareSpoon.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class TagRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Invalid tag type")]
        public TagType Type { get; set; }
    }
}
