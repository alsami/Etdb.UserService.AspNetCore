namespace Etdb.UserService.Authentication.Structures
{
    public class StandardizedAuthError
    {
        public string Message { get; }

        public int Code { get; }

        public StandardizedAuthError(string message, int code)
        {
            this.Message = message;
            this.Code = code;
        }
    }
}