namespace Etdb.UserService.Misc.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AzureServiceBusConfiguration
    {
        public string ConnectionString { get; set; } = null!;

        public string UserRegisteredTopic => "user-registered";

        public string UserAuthenticatedTopic => "user-authenticated";
    }
}