namespace Etdb.UserService.Presentation.Users
{
    public class UserPasswordChangeDto
    {
        public string CurrentPassword { get; }

        public string NewPassword { get; }

        public UserPasswordChangeDto(string currentPassword, string newPassword)
        {
            this.CurrentPassword = currentPassword;
            this.NewPassword = newPassword;
        }
    }
}