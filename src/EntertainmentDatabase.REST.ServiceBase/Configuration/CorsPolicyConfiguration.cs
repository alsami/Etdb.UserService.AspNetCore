namespace EntertainmentDatabase.REST.ServiceBase.Configuration
{
    public sealed class CorsPolicyConfiguration
    {
        public string Name
        {
            get;
            set;
        }

        public bool RegisterGlobally
        {
            get;
            set;
        }

        public bool AllowCredentials
        {
            get;
            set;
        }

        public string[] AllowedOrigins
        {
            get;
            set;
        }

        public string[] AllowedMethods
        {
            get;
            set;
        }

        public string[] AllowedHeaders
        {
            get;
            set;
        }
    }
}
