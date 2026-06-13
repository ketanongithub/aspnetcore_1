using ENyayPath.PICS.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Identity
{
    public interface IIdentityManager
    {
        Task<(IdentityResult Result, User? User)> RegisterAsync(string userName, string email, string password, string? roleName = null);
        Task<LoginResult> LoginAsync(string userName, string password, bool rememberMe);
        Task LogoutAsync();

        // Forgot/Reset password
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByNameAsync(string userName);
        Task<User?> FindByIdAsync(long userId);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);

        // Two-factor & email confirmation
        Task<string> GenerateTwoFactorTokenAsync(User user, string provider);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<IList<string>> GetRolesAsync(long userId);
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public User? User { get; set; }
        public IList<string>? Roles { get; set; }
        public string? ErrorMessage { get; set; }
    }

}
