using ShareSpoon.Domain.Models.Associations;
using ShareSpoon.Domain.Models.BasicEntities;

namespace ShareSpoon.Domain.Models.Ingredients
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
