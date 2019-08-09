namespace Etdb.UserService.Presentation.Users
{
    public class UserPasswordChangeDto
    {
        public string CurrentPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;

        public UserPasswordChangeDto(string currentPassword, string newPassword)
        {
            this.CurrentPassword = currentPassword;
            this.NewPassword = newPassword;
        }

        public UserPasswordChangeDto()
        {
        }
    }
}