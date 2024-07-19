using ShareSpoon.Domain.Models.BasicEntities;
using ShareSpoon.Domain.Models.Recipes;
using ShareSpoon.Domain.Models.Users;

namespace ShareSpoon.Domain.Models.Interactions
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
