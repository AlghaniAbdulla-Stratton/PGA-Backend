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

            var playedMatchIds = playerHistory?.PlayedMatchIds ?? Array.Empty<string>();

            // Step 2: Define a filter to find LongDriveModels not created by the player
            var notPlayerCreatedFilter = Builders<LongDriveModel>.Filter.Ne(model => model.UserId, playerId);

            // If the player has no match history, return any random match not created by the player
            if (playerHistory == null)
            {
                var fallbackModel = await _longdriveCollection.Aggregate()
                    .Match(notPlayerCreatedFilter)
                    .Sample(1)
                    .FirstOrDefaultAsync();

                return (fallbackModel, isReplayed: false);
            }

            // Step 3: Attempt to find a random unplayed LongDriveModel
            var unplayedModelPipeline = new[]
            {
        new BsonDocument("$match", new BsonDocument
        {
            { "UserId", new BsonDocument("$ne", playerId) },
            { "_id", new BsonDocument("$nin", new BsonArray(playedMatchIds.Select(id => new ObjectId(id)))) }
        }),
        new BsonDocument("$sample", new BsonDocument("size", 1)) // Randomly select 1 document
    };

            var unplayedModel = await _longdriveCollection.Aggregate<LongDriveModel>(unplayedModelPipeline).FirstOrDefaultAsync();
            if (unplayedModel != null)
            {
                return (unplayedModel, isReplayed: false);
            }

            // Step 4: Fallback to a previously played game with a "replayed" flag
            var replayedModelPipeline = new[]
            {
        new BsonDocument("$match", new BsonDocument
        {
            { "UserId", new BsonDocument("$ne", playerId) },
            { "_id", new BsonDocument("$in", new BsonArray(playedMatchIds.Select(id => new ObjectId(id)))) }
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
            var playedMatchIds = playerHistory?.PlayedMatchIds?.Select(id => new ObjectId(id)).ToArray() ?? Array.Empty<ObjectId>();

            // Step 2: Attempt to find a LongDriveModel that the player hasn't used before
            var unplayedModelPipeline = new[]
            {
        new BsonDocument("$match", new BsonDocument
        {
            { "MapId", mapId },
            { "UserId", new BsonDocument("$ne", playerId) },
            { "_id", new BsonDocument("$nin", new BsonArray(playedMatchIds)) }
        }),
        new BsonDocument("$sample", new BsonDocument("size", 1)) // Randomly select 1 document
    };

            var unplayedModel = await _longdriveCollection.Aggregate<LongDriveModel>(unplayedModelPipeline).FirstOrDefaultAsync();
            if (unplayedModel != null)
            {
                return (unplayedModel, isReplayed: false);
            }

            // Step 3: If no unplayed model is found, return a random previously played model with a "replayed" flag
            var replayedModelPipeline = new[]
            {
        new BsonDocument("$match", new BsonDocument
        {
            { "MapId", mapId },
            { "UserId", new BsonDocument("$ne", playerId) },
            { "_id", new BsonDocument("$in", new BsonArray(playedMatchIds)) }
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

            // Get the IDs of matches the player has participated in
            var playedMatchIds = playerHistory.PlayedMatchIds;

            // Paginate the match IDs
            var paginatedMatchIds = playedMatchIds
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
