using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class IngredientRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
