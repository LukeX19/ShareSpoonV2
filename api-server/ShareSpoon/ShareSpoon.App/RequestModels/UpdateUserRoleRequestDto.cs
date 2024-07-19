using ShareSpoon.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class UpdateUserRoleRequestDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [Range(0, 2, ErrorMessage = "Invalid role")]
        public AppRole Role { get; set; }
    }
}
