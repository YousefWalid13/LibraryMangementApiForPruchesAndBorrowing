
namespace LibraryManagementAPI.Dto.Account.Reset_Password
{
    public class ResetPasswordResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; internal set; }
    }
}
