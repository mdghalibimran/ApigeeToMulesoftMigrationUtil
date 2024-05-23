using ApigeeToMulesoftMigrationUtil.Models;
using ApigeeToMulesoftMigrationUtil.Providers;
using ApigeeToMulesoftMigrationUtil.Services;

namespace ApigeeToMulesoftMigrationUtil
{
    public class ApigeeMetaDataExtracter : IApigeeMetaDataExtracter
    {
        private readonly IApigeeManagementApiService _apigeeManagementApiService;
        private readonly IConfigService _configService;
        private readonly IApigeeConfiguration _apigeeConfiguration;
        private readonly IBundleProvider _bundleProvider;
        public event PublishStatus? OnStatusPulished;

        public ApigeeMetaDataExtracter(IApigeeManagementApiService apigeeManagementApiService,
            IBundleProvider bundleProvider,
            IConfigService configService)
        {
            if (apigeeManagementApiService == null) throw new ArgumentNullException("IApigeeManagementApiService is null");
            if (configService == null) throw new ArgumentNullException("IConfigService is null");
            if (configService.ApigeeConfiguration == null) throw new ArgumentNullException("IApigeeConfiguration is null");
            if (bundleProvider == null) throw new ArgumentNullException("IBundleProvider is null");

            _apigeeManagementApiService = apigeeManagementApiService;
            _configService = configService;
            _apigeeConfiguration = _configService.ApigeeConfiguration;
            _bundleProvider = bundleProvider;
        }

        public async Task Extract()
        {
            PublishMessage("get the bearer token for Apigee management API ...");
            if (_apigeeConfiguration.Passcode != null)
            {
                _apigeeManagementApiService.AuthenticationToken = _apigeeConfiguration.Passcode;
            }
            else if (_apigeeConfiguration.Username != null && _apigeeConfiguration.Password != null)
            {
                _apigeeManagementApiService.Username = _apigeeConfiguration.Username;
                _apigeeManagementApiService.Password = _apigeeConfiguration.Password;
            }
            else
                throw new Exception("Credentials / AuthenticationToken is not provided in the config");

            if (_apigeeConfiguration.ProxyOrProductName == null)
                throw new NotSupportedException("ProxyOrProductName is not provided in the config");

            if (_apigeeConfiguration.ProxyOrProduct != null && _apigeeConfiguration.ProxyOrProduct.ToLower().Equals("product"))
            {
                var apigeeProduct = await _apigeeManagementApiService.GetApiProductByName(_apigeeConfiguration.ProxyOrProductName);
                var apigeeProductName = apigeeProduct.Name.Trim().Replace(" ", "-").ToLower();
                
                foreach (var proxy in apigeeProduct.Proxies)
                {
                    await DownloadMetaData(_apigeeConfiguration.ProxyOrProductName);
                }
                PublishMessage($"API product {_apigeeConfiguration.ProxyOrProductName} and all API proxies belonging to this product are successfully migrated to Azure APIM!");
            }
            else
            {
                await DownloadMetaData(_apigeeConfiguration.ProxyOrProductName);
            }
            Environment.Exit(0);
        }

        async Task DownloadMetaData(string proxyOrProductName)
        {
            PublishMessage($"Downloading the proxy api '{proxyOrProductName}' bundle...");
            var apiProxyBundle = _bundleProvider.GetApiProxyBundle(proxyOrProductName);
            await apiProxyBundle.LoadBundle();
        }

        private void PublishMessage(string message)
        {
            if(OnStatusPulished != null)
                OnStatusPulished(message);
        }
    }
}