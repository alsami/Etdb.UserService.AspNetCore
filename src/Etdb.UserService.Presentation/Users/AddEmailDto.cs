namespace Etdb.UserService.Presentation.Users
{
    public class AddEmailDto
    {
        public string Address { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public AddEmailDto(string address, bool isPrimary)
        {
            this.Address = address;
            this.IsPrimary = isPrimary;
        }

        public AddEmailDto()
        {
        }
    }
}