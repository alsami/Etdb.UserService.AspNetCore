namespace Etdb.UserService.Authentication.Structures
{
    public class StandardizedAuthErrorContainer
    {
        public StandardizedAuthError Error { get; }

        public StandardizedAuthErrorContainer(StandardizedAuthError error)
        {
            this.Error = error;
        }
    }
}