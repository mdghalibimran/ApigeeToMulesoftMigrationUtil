﻿using ApigeeToMulesoftMigrationUtil.Providers.Bundle;

namespace ApigeeToMulesoftMigrationUtil.Providers
{
    public class ApigeeFileBundleProvider : IBundleProvider
    {
        private readonly IDictionary<string, IBundle> _apiProxyBundles;
        private readonly IDictionary<string, IBundle> _sharedFlowBundles;

        private readonly string _bundleBasePath;

        public ApigeeFileBundleProvider(string bundleBasePath)
        {
            _apiProxyBundles = new Dictionary<string, IBundle>();
            _sharedFlowBundles = new Dictionary<string, IBundle>();

            _bundleBasePath = bundleBasePath;
        }

        public IBundle GetApiProxyBundle(string proxyOrProductName)
        {
            if (!_apiProxyBundles.ContainsKey(proxyOrProductName))
            {
                _apiProxyBundles.Add(proxyOrProductName, new ApigeeFileApiProxyBundle(_bundleBasePath, proxyOrProductName));
            }
            return _apiProxyBundles[proxyOrProductName];
        }

        public IBundle GetSharedFlowBundle(string sharedFlowName)
        {
            if (!_sharedFlowBundles.ContainsKey(sharedFlowName))
            {
                _sharedFlowBundles.Add(sharedFlowName, new ApigeeFileSharedFlowBundle(_bundleBasePath, sharedFlowName));
            }
            return _sharedFlowBundles[sharedFlowName];
        }
    }
}