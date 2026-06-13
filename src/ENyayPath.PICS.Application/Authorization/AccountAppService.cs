using ENyayPath.PICS.Application.Identity.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Identity;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using static ENyayPath.PICS.Application.Identity.Dtos.LoginOutput;

namespace ENyayPath.PICS.Application.Identity
{
    [ApiExplorerSettings(GroupName = "Identity")]
    public class AccountAppService : ApplicationService, IAccountAppService
    {
        private readonly IIdentityManager _identityManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AccountAppService(IIdentityManager identityManager,
                                 IJwtTokenGenerator jwtTokenGenerator,
                                 IAppSession appSession)
            : base(appSession)
        {
            _identityManager = identityManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// OAuth2-compatible token endpoint for form-encoded requests (username + password).
        /// This method supports the OAuth2 password grant flow used by Swagger's Authorize dialog.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenResponse> Token(string username, string password)
        {
            var result = await _identityManager.LoginAsync(username, password, rememberMe: false);
            if (!result.Success)
                throw new Exception(result.ErrorMessage ?? "Authentication failed");

            var token = _jwtTokenGenerator.GenerateJwtToken(result.User!, result.Roles!);
            return new TokenResponse
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = 3600
            };
        }

        [AllowAnonymous]
        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var (result, user) = await _identityManager.RegisterAsync(input.UserName, input.EmailAddress, input.Password, input.RoleName);
            if (!result.Succeeded)
                return new RegisterOutput { Success = false, ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)) };

            var roles = await _identityManager.GetRolesAsync(user!.Id);
            var token = _jwtTokenGenerator.GenerateJwtToken(user, roles);

            return new RegisterOutput { Success = true, UserId = user.Id, Token = token };
        }

        [AllowAnonymous]
        public async Task<LoginOutput> Login(LoginInput input)
        {
            var result = await _identityManager.LoginAsync(input.UserName, input.Password, input.RememberMe);
            if (!result.Success)
                return new LoginOutput { Success = false, ErrorMessage = result.ErrorMessage };

            var token = _jwtTokenGenerator.GenerateJwtToken(result.User!, result.Roles!);
            return new LoginOutput { Success = true, Token = token, ExpiresAt = DateTime.UtcNow.AddHours(1) };
        }

        [Authorize]
        public async Task Logout() => await _identityManager.LogoutAsync();

        public async Task<string> ForgotPassword(ForgotPasswordInput input)
        {
            var user = await _identityManager.FindByEmailAsync(input.EmailAddress);
            if (user == null) throw new Exception("User not found");

            return await _identityManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task ResetPassword(ResetPasswordInput input)
        {
            var user = await _identityManager.FindByEmailAsync(input.EmailAddress);
            if (user == null) throw new Exception("User not found");

            var result = await _identityManager.ResetPasswordAsync(user, input.Token, input.NewPassword);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<string> SendTwoFactorAuthCode(TwoFactorAuthInput input)
        {
            var user = await _identityManager.FindByNameAsync(input.UserName);
            if (user == null) throw new Exception("User not found");

            return await _identityManager.GenerateTwoFactorTokenAsync(user, input.Provider);
        }

        public async Task ConfirmEmail(ConfirmEmailInput input)
        {
            var user = await _identityManager.FindByIdAsync(input.UserId);
            if (user == null) throw new Exception("User not found");

            var result = await _identityManager.ConfirmEmailAsync(user, input.Token);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

}
