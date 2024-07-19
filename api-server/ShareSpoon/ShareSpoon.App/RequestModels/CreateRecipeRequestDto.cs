using ShareSpoon.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class CreateRecipeRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(3000)]
        public string Description { get; set; }

        [Required]
        public TimeSpan EstimatedTime { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "Invalid difficulty level")]
        public DifficultyLevel Difficulty { get; set; }
        public List<RecipeIngredientRequestDto> Ingredients { get; set; }
        public List<RecipeTagRequestDto> Tags { get; set; }
        public string PictureURL { get; set; }
    }
}
