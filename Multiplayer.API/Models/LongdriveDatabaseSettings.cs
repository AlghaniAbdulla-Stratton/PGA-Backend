namespace Multiplayer.API.Models
{
    public class LongdriveDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string LongdriveCollectionName { get; set; } = null!;
        public string MatchRecordsCollectionName { get; set; } = null!;
        public string PlayerMatchHistoryCollectionName { get; set; } = null!;
    }

}
