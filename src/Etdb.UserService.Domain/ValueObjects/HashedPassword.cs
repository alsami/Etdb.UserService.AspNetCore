namespace Etdb.UserService.Domain.ValueObjects
{
    public class HashedPassword
    {
        public HashedPassword(string password, byte[] salt)
        {
            this.Password = password;
            this.Salt = salt;
        }

        public string Password { get; private set; }

        public byte[] Salt { get; private set; }
    }
}