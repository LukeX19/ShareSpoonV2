using ShareSpoon.Domain.Enums;

namespace ShareSpoon.App.ResponseModels
{
    public class TagResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TagType Type { get; set; }
    }
}
