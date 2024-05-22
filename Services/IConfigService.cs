using ApigeeToMulesoftMigrationUtil.Models;

namespace ApigeeToMulesoftMigrationUtil.Services
{
    public interface IConfigService
    {
        ApigeeConfiguration? ApigeeConfiguration { get; }
    }
}