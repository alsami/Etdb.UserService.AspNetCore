namespace Etdb.UserService.Authentication.Structures
{
    internal class GoogleAuthError
    {
        public string Message { get; }

        public int Code { get; }

        public GoogleAuthError(string message, int code)
        {
            this.Message = message;
            this.Code = code;
        }
    }
}