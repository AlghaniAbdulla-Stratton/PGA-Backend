using Multiplayer.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Multiplayer.API.Services
{
    public class LongdriveService
    {
        private readonly IMongoCollection<LongDriveModel> _database;

        public LongdriveService(IOptions<LongdriveDatabaseSettings> longdriveDatabaseSettings)
        {
            var mongoClient = new MongoClient(longdriveDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(longdriveDatabaseSettings.Value.DatabaseName);
            _database = mongoDatabase.GetCollection<LongDriveModel>(longdriveDatabaseSettings.Value.LongdriveCollectionName);
        }

        // Get all long drive data
        public async Task<List<LongDriveModel>> GetAsync() => 
            await _database.Find(i => true).ToListAsync();

        // Get long drive data with ID
        public async Task<LongDriveModel?> GetAsync(string id) =>
            await _database.Find(i => i.id == id).FirstOrDefaultAsync();

        // Get random long drive data
        public async Task<LongDriveModel?> GetRandomAsync()
        {
            var count = await _database.CountDocumentsAsync(new BsonDocument());
            if (count is 0)
                return null;

            var random = new Random();
            var skip = random.Next(0, (int)count);

            return await _database.Find(new BsonDocument()).Skip(skip).Limit(1).FirstOrDefaultAsync();
        }

        // Get random long drive data filtered by map ID
        public async Task<LongDriveModel?> GetRandomByMapAsync(int mapId)
        {
            var filter = Builders<LongDriveModel>.Filter.Eq(i => i.MapId, mapId);
            var count = await _database.CountDocumentsAsync(filter);
            if (count == 0)
                return null;

            var random = new Random();
            var skip = random.Next(0, (int)count);

            return await _database.Find(filter).Skip(skip).Limit(1).FirstOrDefaultAsync();
        }

        // Post new long drive data
        public async Task CreateAsync(LongDriveModel newLongdrive) =>
            await _database.InsertOneAsync(newLongdrive);
    }
}
