
namespace LibraryManagementAPI.Dto.Account.Login
{
    public class LoginResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; // JWT Token
        public bool Success { get; internal set; }
        public string Message { get; internal set; }
        public DateTime Expiration { get; internal set; }
    }
}
