using ShareSpoon.Domain.Enums;

namespace ShareSpoon.App.ResponseModels
{
    public class RecipeTagResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TagType Type { get; set; }
    }
}
