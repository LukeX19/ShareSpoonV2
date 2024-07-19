using ShareSpoon.Domain.Enums;

namespace ShareSpoon.App.ResponseModels
{
    public class AuthenticationResponseDto
    {
        public string Token { get; set; }
        public AppRole Role { get; set; }
    }
}
