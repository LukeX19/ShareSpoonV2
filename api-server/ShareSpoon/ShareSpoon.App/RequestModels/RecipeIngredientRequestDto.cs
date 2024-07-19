using ShareSpoon.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class RecipeIngredientRequestDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Ingredient quantity should be greater than zero")]
        public double Quantity { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Invalid quantity type")]
        public QuantityType QuantityType { get; set; }
    }
}
