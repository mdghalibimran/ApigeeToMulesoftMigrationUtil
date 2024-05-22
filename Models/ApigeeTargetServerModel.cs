namespace ApigeeToMulesoftMigrationUtil.Models
{
    public class ApigeeTargetServerModel
    {
        public string Host { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
        public ApigeeTargetServerSSLInfo SSLInfo { get; set; }
    }
}
