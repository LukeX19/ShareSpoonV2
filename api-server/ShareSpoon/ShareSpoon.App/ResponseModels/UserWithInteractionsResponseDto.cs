using ShareSpoon.Domain.Enums;

namespace ShareSpoon.App.ResponseModels
{
    public class UserWithInteractionsResponseDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public string PictureURL { get; set; }
        public AppRole Role { get; set; }
        public int PostedRecipesCounter { get; set; }
        public int ReceivedLikesCounter { get; set; }
    }
}
