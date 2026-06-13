using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Identity.Dtos
{
    public class RegisterInput
    {
        public string UserName { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? RoleName { get; set; }
    }

    public class RegisterOutput
    {
        public bool Success { get; set; }
        public long? UserId { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class LoginInput
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool RememberMe { get; set; }
    }

    public class LoginOutput
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// OAuth2-compatible token response format.
    /// Used by /api/account/token endpoint for Swagger's Authorize dialog.
    /// </summary>
    public class TokenResponse
    {
        public string access_token { get; set; } = default!;
        public string token_type { get; set; } = "Bearer";
        public int expires_in { get; set; } = 3600;
    }

    public class ForgotPasswordInput
    {
        public string EmailAddress { get; set; } = default!;
    }

    public class ResetPasswordInput
    {
        public string EmailAddress { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }

    public class TwoFactorAuthInput
    {
        public string UserName { get; set; } = default!;
        public string Provider { get; set; } = "Email"; // or SMS
    }

    public class ConfirmEmailInput
    {
        public long UserId { get; set; }
        public string Token { get; set; } = default!;
    }

}
