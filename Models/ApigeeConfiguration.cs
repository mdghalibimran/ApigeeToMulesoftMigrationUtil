namespace ApigeeToMulesoftMigrationUtil.Models
{
    public class ApigeeConfiguration : IApigeeConfiguration
    {
        public ApigeeConfiguration(string organizationName,
            string authenticationBaseUrl,
            string managementBaseUrl,
            string username,
            string password,
            string passcode,
            string proxyOrProduct,
            string proxyOrProductName,
            string environmentName,
            string configDir)
        {
            OrganizationName = organizationName;
            AuthenticationBaseUrl = authenticationBaseUrl;
            ManagementBaseUrl = managementBaseUrl;
            Passcode = passcode;
            ProxyOrProduct = proxyOrProduct;
            ProxyOrProductName = proxyOrProductName;
            EnvironmentName = environmentName;
            ConfigDir = configDir;
            Username = username;
            Password = password;
        }

        public string? OrganizationName { get; private set; }
        public string? AuthenticationBaseUrl { get; private set; }
        public string? ManagementBaseUrl { get; private set; }
        public string? Passcode { get; private set; }
        public string? Username { get; private set; }
        public string? Password { get; private set; }
        public string? ProxyOrProduct { get; private set; }
        public string? ProxyOrProductName { get; private set; }
        public string? EnvironmentName { get; private set; }
        public string? ConfigDir { get; private set; }
    }
}
