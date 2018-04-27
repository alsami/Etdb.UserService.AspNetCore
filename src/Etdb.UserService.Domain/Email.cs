using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Domain
{
    public class Email : GuidDocument
    {
        public string Address { get; set; }

        public bool IsPrimary { get; set; }

        public User User { get; set; }
    }
}