namespace ApigeeToMulesoftMigrationUtil.Poviders.Bundle
{
    public class ApigeeFileApiProxyBundle : IBundle
    {
        private string _bundleBasePath;
        private string _proxyOrProductName;

        public ApigeeFileApiProxyBundle(string bundleBasePath, string proxyOrProductName)
        {
            _bundleBasePath = bundleBasePath;
            _proxyOrProductName = proxyOrProductName;
        }

        public string GetBundlePath()
        {
            if (string.IsNullOrEmpty(_proxyOrProductName))
            {
                throw new Exception("API Proxy bundle not loaded. Please load the bundle first");
            }

            return Path.Combine(_bundleBasePath, _proxyOrProductName, "apiproxy");
        }

        public Task LoadBundle()
        {
            // Nothing to return, the bundle is already in the filesystem
            return Task.CompletedTask;
        }
    }
}