using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tournament.API.Models;

namespace Tournament.API.Services
{
    public class LeaderboardService
    {
        private readonly IMongoCollection<LongdriveScoreModel> _longdrive;

        public LeaderboardService(IOptions<LeaderboardDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _longdrive = database.GetCollection<LongdriveScoreModel>(settings.Value.longdriveBoard);
        }

        #region Longdrive Leaderboard
        public async Task<LongdriveScoreModel?> GetLongdriveEntry(string userId)
        {
            var filter = Builders<LongdriveScoreModel>.Filter.Eq(i => i.ownerId, userId);
            var entry = await _longdrive.Find(filter).FirstOrDefaultAsync();
            if (entry is null)
                return null;

            return entry;
        }
        public async Task<List<LongdriveScoreModel>> GetLongdriveBoard(int page)
        {
            var pageSize = 10;
            var pageIndex = page == 0 ? 1 : page;

            var filter = Builders<LongdriveScoreModel>.Filter.Empty;
            var data = await _longdrive.Find(filter).Sort(Builders<LongdriveScoreModel>.Sort.Descending("score"))
                                              .Skip((pageIndex - 1) * pageSize)
                                              .Limit(pageSize)
                                              .ToListAsync();
            return data;
        }
        public async Task CreateOrUpdateEntry(LongdriveScoreModel score)
        {
            var filter = Builders<LongdriveScoreModel>.Filter.Eq(s => s.ownerId, score.ownerId);
            var entry = await _longdrive.Find(filter).FirstOrDefaultAsync();
            if (entry is null)
            {
                await _longdrive.InsertOneAsync(score);
                return;
            }

            if (entry.timestamp > score.timestamp)
                throw new Exception("Data fed is outdated");
            if (entry.score > score.score)
                throw new Exception("Data fed has lower score");

            var update = Builders<LongdriveScoreModel>.Update.Set("score", score.score)
                                                             .Set("timestamp", score.timestamp);
            var result = await _longdrive.UpdateOneAsync(filter, update);
        }
        #endregion
    }
}
