using ENyayPath.PICS.Core.Authorization.Roles;
using ENyayPath.PICS.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Identity
{
    public class IdentityManager : IIdentityManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public IdentityManager(UserManager<User> userManager,
                               SignInManager<User> signInManager,
                               RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // 🔹 Register
        public async Task<(IdentityResult Result, User? User)> RegisterAsync(string userName, string email, string password, string? roleName = null)
        {
            var user = new User { UserName = userName, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded && !string.IsNullOrWhiteSpace(roleName))
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new Role { Name = roleName });

                await _userManager.AddToRoleAsync(user, roleName);
            }

            return (result, result.Succeeded ? user : null);
        }

        // 🔹 Login
        public async Task<LoginResult> LoginAsync(string userName, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return new LoginResult { Success = false, ErrorMessage = "User not found" };

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: true);
            if (!result.Succeeded)
                return new LoginResult { Success = false, ErrorMessage = "Invalid credentials" };

            var roles = await _userManager.GetRolesAsync(user);
            return new LoginResult { Success = true, User = user, Roles = roles };
        }

        // 🔹 Logout
        public async Task LogoutAsync() => await _signInManager.SignOutAsync();

        // 🔹 User lookups
        public async Task<User?> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        public async Task<User?> FindByIdAsync(long userId) => await _userManager.FindByIdAsync(userId.ToString());
        public async Task<User?> FindByNameAsync(string userName) => await _userManager.FindByNameAsync(userName);

        // 🔹 Password reset
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
            => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
            => await _userManager.ResetPasswordAsync(user, token, newPassword);

        // 🔹 Two-factor auth
        public async Task<string> GenerateTwoFactorTokenAsync(User user, string provider)
            => await _userManager.GenerateTwoFactorTokenAsync(user, provider);

        // 🔹 Email confirmation
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
            => await _userManager.ConfirmEmailAsync(user, token);

        // 🔹 Roles
        public async Task<IList<string>> GetRolesAsync(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user != null ? await _userManager.GetRolesAsync(user) : new List<string>();
        }

        public async Task<IdentityResult> AssignRoleAsync(long userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new Role { Name = roleName });

            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }

}
