using System.ComponentModel.DataAnnotations;

namespace ShareSpoon.App.RequestModels
{
    public class LikeRequestDto
    {
        [Required]
        public long RecipeId { get; set; }
    }
}
