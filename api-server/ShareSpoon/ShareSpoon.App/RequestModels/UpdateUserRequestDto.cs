using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class UpdateUserRequestDto
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
    }
}
