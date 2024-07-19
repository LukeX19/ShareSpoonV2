using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Ingredients;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.Domain.Models.Associations
{
    public class RecipeIngredient
    {
        public double Quantity { get; set; }
        public QuantityType QuantityType { get; set; }
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public long IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
