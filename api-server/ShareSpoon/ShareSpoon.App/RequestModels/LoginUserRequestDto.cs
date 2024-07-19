using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class LoginUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
