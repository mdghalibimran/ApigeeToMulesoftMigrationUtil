namespace ApigeeToMulesoftMigrationUtil.Models
{
    public class AppSettings
    {
        public ApigeeConfiguration Apigee { get; set; }
        public ExportConfiguration Export { get; set; }
    }

    public class ExportConfiguration
    {
        public string Folder { get; set; }
    }
}
