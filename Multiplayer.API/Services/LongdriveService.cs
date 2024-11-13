using Multiplayer.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using PGALegends.Models;

namespace Multiplayer.API.Services
{
    public class LongdriveService
    {
        private readonly IMongoCollection<LongDriveModel> _longdriveCollection;
        private readonly IMongoCollection<MatchRecord> _matchRecordCollection;
        private readonly IMongoCollection<PlayerMatchHistory> _playerMatchHistoryCollection;

        public LongdriveService(IOptions<LongdriveDatabaseSettings> longdriveDatabaseSettings)
        {
            var mongoClient = new MongoClient(longdriveDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(longdriveDatabaseSettings.Value.DatabaseName);
            _longdriveCollection = mongoDatabase.GetCollection<LongDriveModel>(longdriveDatabaseSettings.Value.LongdriveCollectionName);
            _matchRecordCollection = mongoDatabase.GetCollection<MatchRecord>("match_records");
            _playerMatchHistoryCollection = mongoDatabase.GetCollection<PlayerMatchHistory>("players_match_history");
        }

        // Get all long drive data
        public async Task<List<LongDriveModel>> GetAsync() =>
            await _longdriveCollection.Find(i => true).ToListAsync();

        // Get long drive data with ID
        public async Task<LongDriveModel?> GetAsync(string id) =>
            await _longdriveCollection.Find(i => i.id == id).FirstOrDefaultAsync();

        // Get random long drive data
        public async Task<LongDriveModel?> GetRandomAsync()
        {
            var count = await _longdriveCollection.CountDocumentsAsync(new BsonDocument());
            if (count == 0)
                return null;

            var random = new Random();
            var skip = random.Next(0, (int)count);

            return await _longdriveCollection.Find(new BsonDocument()).Skip(skip).Limit(1).FirstOrDefaultAsync();
        }

        // Get random long drive data filtered by map ID
        public async Task<LongDriveModel?> GetRandomByMapAsync(int mapId)
        {
            var filter = Builders<LongDriveModel>.Filter.Eq(i => i.MapId, mapId);
            var count = await _longdriveCollection.CountDocumentsAsync(filter);
            if (count == 0)
                return null;

            var random = new Random();
            var skip = random.Next(0, (int)count);

            return await _longdriveCollection.Find(filter).Skip(skip).Limit(1).FirstOrDefaultAsync();
        }

        // Post new long drive data
        public async Task CreateAsync(LongDriveModel newLongdrive) =>
            await _longdriveCollection.InsertOneAsync(newLongdrive);

        // Upload new match record
        public async Task UploadMatchRecordAsync(MatchRecord matchRecord) =>
            await _matchRecordCollection.InsertOneAsync(matchRecord);

        // Get paginated match history for a specific player
        // Method to get a player's match history
        public async Task<List<MatchRecord>> GetPaginatedMatchHistoryAsync(string playerId, int pageNumber, int pageSize)
        {
            // Find the player's match history document
            var playerHistory = await _playerMatchHistoryCollection
                .Find(history => history.UserId == playerId)
                .FirstOrDefaultAsync();

            // If no history exists, return an empty list
            if (playerHistory == null || playerHistory.PlayedMatchIds == null)
                return new List<MatchRecord>();

            // Get the IDs of matches the player has participated in
            var playedMatchIds = playerHistory.PlayedMatchIds;

            // Paginate the match IDs
            var paginatedMatchIds = playedMatchIds
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Fetch the MatchRecord documents for the paginated IDs
            var filter = Builders<MatchRecord>.Filter.In(record => record._id, paginatedMatchIds);
            return await _matchRecordCollection.Find(filter).ToListAsync();
        }

        // Add or update player match history for a specific match
        public async Task AddOrUpdatePlayerMatchHistoryAsync(string playerId, string matchId)
        {
            var filter = Builders<PlayerMatchHistory>.Filter.Eq(history => history.UserId, playerId);
            var update = Builders<PlayerMatchHistory>.Update.AddToSet(history => history.PlayedMatchIds, matchId);

            await _playerMatchHistoryCollection.UpdateOneAsync(
                filter,
                update,
                new UpdateOptions { IsUpsert = true } // Create document if it doesn't exist
            );
        }
    }
}
