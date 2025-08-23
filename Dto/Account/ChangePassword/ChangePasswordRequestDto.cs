using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Dto.Account.ChangePassword
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
