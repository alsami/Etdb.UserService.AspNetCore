namespace Etdb.UserService.Authentication.Structures
{
    internal class GoogleAuthErrorContainer
    {
        public GoogleAuthError Error { get; }

        public GoogleAuthErrorContainer(GoogleAuthError error)
        {
            this.Error = error;
        }
    }
}