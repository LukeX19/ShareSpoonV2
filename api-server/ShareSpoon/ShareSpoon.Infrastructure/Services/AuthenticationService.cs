using Microsoft.AspNetCore.Identity;
using ShareSpoon.App.Abstractions;
using ShareSpoon.Domain.Models.Users;
using ShareSpoon.Infrastructure.Exceptions;

namespace ShareSpoon.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthenticationService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<AppUser> Register(AppUser newUser, string password, CancellationToken ct = default)
        {
            var foundUser = await _userManager.FindByEmailAsync(newUser.Email);
            if (foundUser != null)
            {
                throw new UserAlreadyExistsException(newUser.Email);
            }

            var identity = new AppUser
            {
                Email = newUser.Email,
                UserName = newUser.UserName,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Birthday = newUser.Birthday,
                PictureURL = newUser.PictureURL,
                Role = newUser.Role
            };

            var registerResult = await _userManager.CreateAsync(identity, password);

            var foundRole = await _roleManager.FindByNameAsync(newUser.Role.ToString());
            if (foundRole == null)
            {
                var newRole = new IdentityRole(newUser.Role.ToString());
                await _roleManager.CreateAsync(newRole);
            }

            await _userManager.AddToRoleAsync(identity, newUser.Role.ToString());

            if (!registerResult.Succeeded)
            {
                throw new Exception("Registration failed!");
            }

            return identity;
        }

        public async Task<AppUser> Login(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!loginResult.Succeeded)
            {
                throw new InvalidCredentialsException();
            }

            return user;
        }

        public async Task DeleteAccount(string id, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException("User", id);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Account deletion failed!");
            }
        }
    }
}
