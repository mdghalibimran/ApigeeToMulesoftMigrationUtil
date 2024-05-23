using ApigeeToMulesoftMigrationUtil.Providers.Bundle;

namespace ApigeeToMulesoftMigrationUtil.Providers
{
    public interface IBundleProvider
    {
        IBundle GetApiProxyBundle(string proxyOrProductName);
        IBundle GetSharedFlowBundle(string sharedFlowName);
    }
}