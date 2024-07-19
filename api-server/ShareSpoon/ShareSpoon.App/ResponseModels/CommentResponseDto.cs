namespace ShareSpoon.App.ResponseModels
{
    public class CommentResponseDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public UserResponseDto User { get; set; }
        public long RecipeId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
