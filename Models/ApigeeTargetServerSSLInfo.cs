namespace ApigeeToMulesoftMigrationUtil.Models
{
    public class ApigeeTargetServerSSLInfo
    {
        public ICollection<string> Ciphers { get; set; }
        public bool ClientAuthEnabled { get; set; }
        public bool Enabled { get; set; }
        public bool IgnoreValidationError { get; set; }
        public ICollection<string> Protocols { get; set; }
        public string TrustStore { get; set; }
    }
}
