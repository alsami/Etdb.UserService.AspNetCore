namespace Etdb.UserService.Presentation.Users
{
    public class AddEmailDto
    {
        public string Address { get; }

        public bool IsPrimary { get; }

        public AddEmailDto(string address, bool isPrimary)
        {
            this.Address = address;
            this.IsPrimary = isPrimary;
        }
    }
}