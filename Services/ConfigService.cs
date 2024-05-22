using ApigeeToMulesoftMigrationUtil.Models;
using System.Text.Json;

namespace ApigeeToMulesoftMigrationUtil.Services
{
    public class ConfigService : IConfigService
    {
        private const string ConfigFileName = "appSettings.json";
        
        public ConfigService()
        {
            if (!File.Exists(ConfigFileName))
                throw new FileNotFoundException(ConfigFileName);

            AppSettings? appSettings;

            using (StreamReader reader = new StreamReader(ConfigFileName))
            {
                var settings = reader.ReadToEnd();
                appSettings = JsonSerializer.Deserialize<AppSettings>(settings);                
            }

            if (appSettings == null)
                throw new NotSupportedException("Invalid Configration File");

            ApigeeConfiguration = appSettings.Apigee;
            ExportConfiguration = appSettings.Export;
        }

        public ApigeeConfiguration ApigeeConfiguration { get; private set; }
        public ExportConfiguration ExportConfiguration { get; private set; }

        public bool ValiadteConfig()
        {
            if (string.IsNullOrEmpty(ExportConfiguration.Folder))
                throw new NotSupportedException("Export Location is not provided");

            if (string.IsNullOrEmpty(ApigeeConfiguration.AuthenticationBaseUrl))
                throw new NotSupportedException("AuthenticationBaseUrl is not provided");

            if (string.IsNullOrEmpty(ApigeeConfiguration.ManagementBaseUrl))
                throw new NotSupportedException("ManagementBaseUrl is not provided");

            if (string.IsNullOrEmpty(ApigeeConfiguration.Passcode)
                && (string.IsNullOrEmpty(ApigeeConfiguration.Username) || string.IsNullOrEmpty(ApigeeConfiguration.Password)))
                throw new NotSupportedException("Either 'Username & Password' / 'Passcode' are required");

            if (string.IsNullOrEmpty(ApigeeConfiguration.ProxyOrProduct)
                || (ApigeeConfiguration.ProxyOrProduct != "Proxy" && ApigeeConfiguration.ProxyOrProduct != "Product"))
                throw new NotSupportedException("Provide ProxyOrProduct as valid options 'Proxy' / 'Product'");

            if (string.IsNullOrEmpty(ApigeeConfiguration.ProxyOrProductName))
                throw new NotSupportedException("ProxyOrProductName is not provided");

            return true;
        }
    }
}
