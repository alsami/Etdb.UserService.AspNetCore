using Etdb.UserService.Domain.Base;

namespace Etdb.UserService.Domain
{
    public class SecurityRole : GuidDocument
    {
        public string Name { get; set; }
    }
}