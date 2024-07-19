using Microsoft.AspNetCore.Identity;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Interactions;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.Domain.Models.Users
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string PictureURL { get; set; }
        public AppRole Role { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
