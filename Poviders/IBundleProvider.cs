using ApigeeToMulesoftMigrationUtil.Poviders.Bundle;

namespace ApigeeToMulesoftMigrationUtil.Poviders
{
    public interface IBundleProvider
    {
        IBundle GetApiProxyBundle(string proxyOrProductName);
        IBundle GetSharedFlowBundle(string sharedFlowName);
    }
}