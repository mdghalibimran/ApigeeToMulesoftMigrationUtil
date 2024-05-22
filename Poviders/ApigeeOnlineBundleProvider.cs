using ApigeeToMulesoftMigrationUtil.Poviders.Bundle;
using ApigeeToMulesoftMigrationUtil.Services;

namespace ApigeeToMulesoftMigrationUtil.Poviders
{
    public class ApigeeOnlineBundleProvider : IBundleProvider
    {
        private readonly IApigeeManagementApiService _apigeeManagementApiService;
        private readonly IDictionary<string, IBundle> _apiProxyBundles;
        private readonly IDictionary<string, IBundle> _sharedFlowBundles;

        private readonly string _bundleBasePath;

        public ApigeeOnlineBundleProvider(string bundleBasePath, IApigeeManagementApiService apigeeManagementApiService)
        {
            _apigeeManagementApiService = apigeeManagementApiService;
            _apiProxyBundles = new Dictionary<string, IBundle>();
            _sharedFlowBundles = new Dictionary<string, IBundle>();

            _bundleBasePath = bundleBasePath;
        }

        public IBundle GetApiProxyBundle(string proxyOrProductName)
        {
            if (!_apiProxyBundles.ContainsKey(proxyOrProductName))
            {
                _apiProxyBundles.Add(proxyOrProductName, new ApigeeOnlineApiProxyBundle(_bundleBasePath, proxyOrProductName, _apigeeManagementApiService));
            }
            return _apiProxyBundles[proxyOrProductName];
        }

        public IBundle GetSharedFlowBundle(string sharedFlowName)
        {
            if (!_sharedFlowBundles.ContainsKey(sharedFlowName))
            {
                _sharedFlowBundles.Add(sharedFlowName, new ApigeeOnlineSharedFlowBundle(_bundleBasePath, sharedFlowName, _apigeeManagementApiService));
            }
            return _sharedFlowBundles[sharedFlowName];
        }
    }
}
