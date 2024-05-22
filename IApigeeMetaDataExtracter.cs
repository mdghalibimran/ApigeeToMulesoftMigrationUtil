namespace ApigeeToMulesoftMigrationUtil
{
    public delegate void PublishStatus(string msg);

    public interface IApigeeMetaDataExtracter
    {
        Task Extract();
        event PublishStatus? OnStatusPulished;
    }
}