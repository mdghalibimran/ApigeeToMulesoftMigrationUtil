using ApigeeToMulesoftMigrationUtil.Models;

namespace ApigeeToMulesoftMigrationUtil.Services
{
    public interface IApigeeManagementApiService
    {
        string AuthenticationToken { get; set; }
        string? Environment { get; }
        string Password { get; set; }
        string ProxyName { get; }
        string Username { get; set; }

        Task<string> DownloadApiProxyBundle(string basePath, string proxyName, int revision);
        Task<string> DownloadSharedFlowBundle(string basePath, string sharedFlowName, int revision);
        Task<ApiProductMetaData> GetApiProductByName(string productName);
        Task<ApiProductMetaData> GetApiProducts();
        Task<ApigeeEntityModel> GetApiProxyByName(string proxyName);
        Task<KeyValueMapModel> GetKeyValueMapByName(string proxyName, string environment, string mapIdentifier);
        Task<ApigeeEntityModel> GetSharedFlowByName(string sharedFlowName);
        Task<ApigeeTargetServerModel> GetTargetServerByName(string targetServerName, string environment);
    }
}