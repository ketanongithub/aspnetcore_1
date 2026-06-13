using ENyayPath.PICS.Application.Identity.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Identity
{
    public interface IAccountAppService
    {
        // 🔹 Registration
        Task<RegisterOutput> Register(RegisterInput input);

        // 🔹 Login / Logout
        Task<LoginOutput> Login(LoginInput input);
        
        /// <summary>
        /// OAuth2-compatible token endpoint for form-encoded requests (used by Swagger's Authorize)
        /// Accepts: username, password, grant_type parameters
        /// Returns: token for use in Authorization header
        /// </summary>
        Task<TokenResponse> Token(string username, string password);
        
        Task Logout();

        // 🔹 Password reset
        Task<string> ForgotPassword(ForgotPasswordInput input);
        Task ResetPassword(ResetPasswordInput input);

        // 🔹 Two-factor authentication
        Task<string> SendTwoFactorAuthCode(TwoFactorAuthInput input);

        // 🔹 Email confirmation
        Task ConfirmEmail(ConfirmEmailInput input);
    }
}
