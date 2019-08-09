namespace Etdb.UserService.Presentation.Users
{
    public class UserNameAvailabilityDto
    {
        public bool Available { get; set; }

        public UserNameAvailabilityDto(bool available)
        {
            this.Available = available;
        }

        public UserNameAvailabilityDto()
        {
        }
    }
}