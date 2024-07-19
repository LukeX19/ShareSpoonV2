namespace ShareSpoon.App.ResponseModels
{
    public class LikeResponseDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public long RecipeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
