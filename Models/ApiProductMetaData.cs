namespace ApigeeToMulesoftMigrationUtil.Models
{
    public class ApiProductMetaData
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public IList<string> Proxies { get; set; }
    }
}
