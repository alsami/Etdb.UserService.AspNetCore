namespace Etdb.UserService.Cqrs.Abstractions.Base
{
    public abstract class PasswordCommand
    {
        public string Password { get; set; }
    }
}