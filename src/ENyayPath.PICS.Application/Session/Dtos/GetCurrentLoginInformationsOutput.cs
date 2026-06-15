using System;

namespace ENyayPath.PICS.Application.Session.Dtos
{
    public class GetCurrentLoginInformationsOutput
    {
        public UserLoginInfoDto? User { get; set; }
    }

    public class UserLoginInfoDto
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? AliasName { get; set; }
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
