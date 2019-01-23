namespace Etdb.UserService.Authentication.Structures
{
    internal class StandardizedAuthErrorContainer
    {
        public StandardizedAuthError Error { get; }

        public StandardizedAuthErrorContainer(StandardizedAuthError error)
        {
            this.Error = error;
        }
    }
}