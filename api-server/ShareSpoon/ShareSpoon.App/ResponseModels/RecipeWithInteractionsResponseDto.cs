using ShareSpoon.Domain.Enums;

namespace ShareSpoon.App.ResponseModels
{
    public class RecipeWithInteractionsResponseDto
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public UserResponseDto User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public ICollection<RecipeIngredientResponseDto> RecipeIngredients { get; set; }
        public ICollection<RecipeTagResponseDto> RecipeTags { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PictureURL { get; set; }
        public int LikesCounter { get; set; }
        public bool CurrentUserLiked { get; set; }
        public int CommentsCounter { get; set; }
    }
}
