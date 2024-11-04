namespace Account.API.Models.Profile
{
    public class ProfileDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ProfileCollectionName { get; set; } = null!;
    }
}
