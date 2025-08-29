namespace LibraryManagementAPI.Dto.Account.Register
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
       
    }
}
