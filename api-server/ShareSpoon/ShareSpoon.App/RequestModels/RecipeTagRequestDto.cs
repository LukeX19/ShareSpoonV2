using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class RecipeTagRequestDto
    {
        [Required]
        public long Id { get; set; }
    }
}
