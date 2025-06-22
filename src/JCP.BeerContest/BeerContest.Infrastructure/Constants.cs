namespace BeerContest.Infrastructure
{
    public static class Constants
    {
        public static class KeyVault
        {
            /// <summary>
            /// AKV configuration
            /// </summary>
            public static class ConfigurationKeys
            {
                /// <summary>
                /// AKV url
                /// </summary>
                public const string URL = "KeyVault:BaseUrl";
            }
        }

        public static class Google
        {
            public static class Authentication
            {
                public const string CLIENT_ID = "Authentication:Google:ClientId";
                public const string CLIENT_SECRET = "Authentication:Google:ClientSecret";
            }
        }

        public static class GoogleCloud
        {
            public const string PROJECT_ID = "GoogleCloud:ProjectId";
        }

        public static class Firebase
        {
            public const string CREDENTIALS = "Firebase:Service:Account";
        }
    }
}
