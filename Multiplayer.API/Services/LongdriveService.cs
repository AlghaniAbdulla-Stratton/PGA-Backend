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
            _matchRecordCollection = mongoDatabase.GetCollection<MatchRecord>(longdriveDatabaseSettings.Value.MatchRecordsCollectionName);
            _playerMatchHistoryCollection = mongoDatabase.GetCollection<PlayerMatchHistory>(longdriveDatabaseSettings.Value.PlayerMatchHistoryCollectionName);
        }


        // Get all long drive data
        public async Task<List<LongDriveModel>> GetAsync() =>
            await _longdriveCollection.Find(i => true).ToListAsync();

        // Get long drive data with ID
        public async Task<LongDriveModel?> GetAsync(string id) =>
            await _longdriveCollection.Find(i => i.id == id).FirstOrDefaultAsync();

        public async Task<(LongDriveModel? longDriveModel, bool isReplayed)> GetRandomLongdriveAsync(string playerId)
        {
            // Step 1: Retrieve the player's match history to find games they’ve already played
            var playerHistory = await _playerMatchHistoryCollection
                .Find(history => history.UserId == playerId)
                .FirstOrDefaultAsync();

            // If the player has no history, initialize `playedMatchIds` as an empty array
            var playedMatchRecordIdsAsObjectId = playerHistory?.PlayedMatchIds
                ?.Select(ObjectId.Parse) // Convert PlayedMatchIds from strings to ObjectId
                .ToArray() ?? Array.Empty<ObjectId>();

            // Step 2: Fetch MatchRecord documents using `PlayedMatchIds`
            var matchRecordFilter = Builders<MatchRecord>.Filter.In(
                record => record.id, // Assuming `record.id` is an ObjectId
                playedMatchRecordIdsAsObjectId
            );

            var matchRecords = await _matchRecordCollection.Find(matchRecordFilter).ToListAsync();

            // Extract all `Player1MatchDataId` and `Player2MatchDataId` to check if games have already been played
            var previouslyPlayedMatchDataIds = matchRecords
                .SelectMany(record => new[] { record.Player1MatchDataId, record.Player2MatchDataId })
                .ToArray();

            // Step 3: Attempt to find a LongDriveModel that the player hasn't used before
            var unplayedModelPipeline = new[]
            {
        new BsonDocument("$match", new BsonDocument
        {
            { "UserId", new BsonDocument("$ne", playerId) },
            { "_id", new BsonDocument("$nin", new BsonArray(previouslyPlayedMatchDataIds.Select(ObjectId.Parse))) }
        }),
        new BsonDocument("$sample", new BsonDocument("size", 1)) // Randomly select 1 document
    };

            var unplayedModel = await _longdriveCollection.Aggregate<LongDriveModel>(unplayedModelPipeline).FirstOrDefaultAsync();
            if (unplayedModel != null)
            {
                return (unplayedModel, isReplayed: false);
            }

            // Step 4: If no unplayed model is found, return a random previously played model with a "replayed" flag
            var replayedModelPipeline = new[]
            {
        new BsonDocument("$match", new BsonDocument
        {
            { "UserId", new BsonDocument("$ne", playerId) },
            { "_id", new BsonDocument("$in", new BsonArray(previouslyPlayedMatchDataIds.Select(ObjectId.Parse))) }
        }),
        new BsonDocument("$sample", new BsonDocument("size", 1)) // Randomly select 1 document
    };

            var replayedModel = await _longdriveCollection.Aggregate<LongDriveModel>(replayedModelPipeline).FirstOrDefaultAsync();
            return (replayedModel, isReplayed: true);
        }





        // Get random long drive data filtered by map ID
        public async Task<(LongDriveModel? longDriveModel, bool isReplayed)> GetRandomLongdriveByMapAsync(int mapId, string playerId)
        {
            // Step 1: Retrieve the player's match history to find games they’ve already played
            var playerHistory = await _playerMatchHistoryCollection
                .Find(history => history.UserId == playerId)
                .FirstOrDefaultAsync();

            // If the player has no history, initialize `playedMatchIds` as an empty array
            var playedMatchRecordIdsAsObjectId = playerHistory?.PlayedMatchIds
                ?.Select(ObjectId.Parse) // Convert PlayedMatchIds from strings to ObjectId
                .ToArray() ?? Array.Empty<ObjectId>();

            // Step 2: Fetch MatchRecord documents using `PlayedMatchIds`
            var matchRecordFilter = Builders<MatchRecord>.Filter.In(
                record => record.id, // Assuming `record.id` is an ObjectId
                playedMatchRecordIdsAsObjectId
            );

            var matchRecords = await _matchRecordCollection.Find(matchRecordFilter).ToListAsync();

            // Extract all `Player1MatchDataId` and `Player2MatchDataId` to check if games have already been played
            var previouslyPlayedMatchDataIds = matchRecords
                .SelectMany(record => new[] { record.Player1MatchDataId, record.Player2MatchDataId })
                .ToArray();

            // Step 3: Attempt to find a LongDriveModel that the player hasn't used before
            var unplayedModelPipeline = new[]
            {
        new BsonDocument("$match", new BsonDocument
        {
            { "MapId", mapId },
            { "UserId", new BsonDocument("$ne", playerId) },
            { "_id", new BsonDocument("$nin", new BsonArray(previouslyPlayedMatchDataIds.Select(ObjectId.Parse))) }
        }),
        new BsonDocument("$sample", new BsonDocument("size", 1)) // Randomly select 1 document
    };

            var unplayedModel = await _longdriveCollection.Aggregate<LongDriveModel>(unplayedModelPipeline).FirstOrDefaultAsync();
            if (unplayedModel != null)
            {
                return (unplayedModel, isReplayed: false);
            }

            // Step 4: If no unplayed model is found, return a random previously played model with a "replayed" flag
            var replayedModelPipeline = new[]
            {
        new BsonDocument("$match", new BsonDocument
        {
            { "MapId", mapId },
            { "UserId", new BsonDocument("$ne", playerId) },
            { "_id", new BsonDocument("$in", new BsonArray(previouslyPlayedMatchDataIds.Select(ObjectId.Parse))) }
        }),
        new BsonDocument("$sample", new BsonDocument("size", 1)) // Randomly select 1 document
    };

            var replayedModel = await _longdriveCollection.Aggregate<LongDriveModel>(replayedModelPipeline).FirstOrDefaultAsync();
            return (replayedModel, isReplayed: true);
        }





        // Post new long drive data
        public async Task CreateAsync(LongDriveModel newLongdrive) =>
            await _longdriveCollection.InsertOneAsync(newLongdrive);

        // Upload new match record
        public async Task UploadMatchRecordAsync(MatchRecord matchRecord) =>
            await _matchRecordCollection.InsertOneAsync(matchRecord);

        // Get paginated match history for a specific player
        // Method to get a player's match history
        public async Task<MatchHistoryResponse> GetPaginatedMatchHistoryAsync(string playerId, int pageNumber, int pageSize)
        {
            // Find the player's match history document
            var playerHistory = await _playerMatchHistoryCollection
                .Find(history => history.UserId == playerId)
                .FirstOrDefaultAsync();

            // If no history exists, return an empty MatchHistoryResponse
            if (playerHistory == null || playerHistory.PlayedMatchIds == null)
            {
                return new MatchHistoryResponse
                {
                    MatchRecords = Array.Empty<MatchRecord>()
                };
            }

            // Get the IDs of matches the player has participated in as ObjectId
            var playedMatchIdsAsObjectIds = playerHistory.PlayedMatchIds
                .Select(ObjectId.Parse)
                .ToArray();

            // Paginate the match IDs
            var paginatedMatchIds = playedMatchIdsAsObjectIds
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Fetch the MatchRecord documents for the paginated IDs
            var filter = Builders<MatchRecord>.Filter.In(record => record.id, paginatedMatchIds);
            var matchRecords = await _matchRecordCollection.Find(filter).ToListAsync();

            // Return the MatchHistoryResponse object
            return new MatchHistoryResponse
            {
                MatchRecords = matchRecords.ToArray()
            };
        }



        // Add or update player match history for a specific match
        public async Task AddOrUpdatePlayerMatchHistoryAsync(string playerId, string? matchId)
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
