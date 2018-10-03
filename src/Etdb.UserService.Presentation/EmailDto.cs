using Etdb.UserService.Presentation.Base;

namespace Etdb.UserService.Presentation
{
    public class EmailDto : GuidDto
    {
        public string Address { get; set; }

        public bool IsPrimary { get; set; }
    }
}