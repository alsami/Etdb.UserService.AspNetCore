namespace Etdb.UserService.Presentation.Users
{
    public class UserNameAvailabilityDto
    {
        public bool Available { get; }

        public UserNameAvailabilityDto(bool available)
        {
            this.Available = available;
        }
    }
}