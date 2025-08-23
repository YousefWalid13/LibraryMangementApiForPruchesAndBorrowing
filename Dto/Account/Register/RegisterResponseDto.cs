
namespace LibraryManagementAPI.Dto.Account.Register
{
    public class RegisterResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; // JWT Token مثلاً
        public bool Success { get; internal set; }
        public string Message { get; internal set; }
        public List<string> Errors { get; internal set; }
    }
}
