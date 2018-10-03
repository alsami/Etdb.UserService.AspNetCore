namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class PasswordCommand
    {
        protected PasswordCommand(string newPassword)
        {
            NewPassword = newPassword;
        }

        public string NewPassword { get; }
    }
}