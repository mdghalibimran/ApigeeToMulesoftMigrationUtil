namespace ApigeeToMulesoftMigrationUtil.Models
{
    public class KeyValueMapModel
    {
        public bool Encrypted { get; set; }
        public string Name { get; set; }
        public ICollection<KeyValueMapItemModel> Entry { get; set; }
    }
}
