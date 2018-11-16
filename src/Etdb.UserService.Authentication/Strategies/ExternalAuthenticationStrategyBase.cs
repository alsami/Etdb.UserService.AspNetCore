namespace Etdb.UserService.Authentication.Strategies
{
    public abstract class ExternalAuthenticationStrategyBase
    {
        protected abstract string UserProfileUrl { get; }
    }
}