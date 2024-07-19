using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Associations;
using ShareSpoon.Domain.Models.BasicEntities;
using ShareSpoon.Domain.Models.Interactions;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.Domain.Models.Recipes
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PictureURL { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public ICollection<RecipeTag> RecipeTags { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
