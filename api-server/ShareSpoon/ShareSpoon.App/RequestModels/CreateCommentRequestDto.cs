using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class CreateCommentRequestDto
    {
        [Required]
        public long RecipeId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
}
