namespace ApigeeToMulesoftMigrationUtil.Providers.Bundle
{
    public interface IBundle
    {
        Task LoadBundle();
        string GetBundlePath();
    }
}
