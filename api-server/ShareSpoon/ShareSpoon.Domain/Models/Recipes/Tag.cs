using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Associations;
using ShareSpoon.Domain.Models.BasicEntities;

namespace ShareSpoon.Domain.Models.Recipes
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public TagType Type { get; set; }
        public ICollection<RecipeTag> RecipeTags { get; set; }
    }
}
