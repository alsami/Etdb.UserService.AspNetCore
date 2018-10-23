using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Cqrs.Abstractions.Commands;
using Etdb.UserService.Cqrs.Validation.Base;
using Etdb.UserService.Services.Abstractions;
using FluentValidation;

namespace Etdb.UserService.Cqrs.Validation.Passwords
{
    public class PasswordChangeCommandValidation : PasswordCommandValidation<UserPasswordChangeCommand>
    {
        private readonly IUsersService usersService;
        private readonly IHasher hasher;

        public PasswordChangeCommandValidation(IUsersService usersService, IHasher hasher)
        {
            this.usersService = usersService;
            this.hasher = hasher;

            this.RegisterDefaultPasswordRule("New password");
            this.RegisterCurrentPasswordRules();
        }

        private void RegisterCurrentPasswordRules()
        {
            this.RuleFor(command => command)
                .MustAsync(async (command, _) =>
                    await this.CurrentPasswordMatchesUsersPasswordAsync(command))
                .WithMessage("User or password invalid!");

            this.RuleFor(command => command.CurrentPassword)
                .MinimumLength(this.PasswordMinLength)
                .WithMessage(string.Format(this.PasswordTooShortMessage, "Current password"));
        }

        private async Task<bool> CurrentPasswordMatchesUsersPasswordAsync(UserPasswordChangeCommand command)
        {
            var user = await this.usersService.FindByIdAsync(command.Id);

            return this.hasher.CreateSaltedHash(command.CurrentPassword, user.Salt) == user.Password;
        }
    }
}