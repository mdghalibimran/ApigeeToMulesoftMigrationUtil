namespace ApigeeToMulesoftMigrationUtil.Models
{
    public interface IApigeeConfiguration
    {
        string? AuthenticationBaseUrl { get; }
        string? ConfigDir { get; }
        string? EnvironmentName { get; }
        string? ManagementBaseUrl { get; }
        string? OrganizationName { get; }
        string? Passcode { get; }
        string? Password { get; }
        string? ProxyOrProduct { get; }
        string? ProxyOrProductName { get; }
        string? Username { get; }
    }
}