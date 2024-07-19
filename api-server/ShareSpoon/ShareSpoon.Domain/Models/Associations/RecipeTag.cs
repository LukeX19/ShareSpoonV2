using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.Domain.Models.Associations
{
    public class RecipeTag
    {
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public long TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
