namespace LibraryManagementAPI.Dto.Account.ChangePassword
{
    public class ChangePasswordResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Errors { get; internal set; }
    }
}
