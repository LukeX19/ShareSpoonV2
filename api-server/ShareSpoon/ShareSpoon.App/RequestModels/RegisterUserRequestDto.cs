using ShareSpoon.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class RegisterUserRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string PictureURL { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Range(0, 2, ErrorMessage = "Invalid role")]
        public AppRole Role { get; set; }
    }
}
