namespace ApigeeToMulesoftMigrationUtil.Poviders.Bundle
{
    public interface IBundle
    {
        Task LoadBundle();
        string GetBundlePath();
    }
}
