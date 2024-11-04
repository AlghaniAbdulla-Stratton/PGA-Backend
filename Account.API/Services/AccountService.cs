using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Account.API.Models.Profile;
using Account.API.Models.Profile.Utils;

namespace Account.API.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<ProfileModel> _profiles;


        public AccountService(IOptions<ProfileDatabaseSettings> profileDatabaseSettings)
        {
            var mongoClient = new MongoClient(profileDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(profileDatabaseSettings.Value.DatabaseName);
            _profiles = mongoDatabase.GetCollection<ProfileModel>(profileDatabaseSettings.Value.ProfileCollectionName);
        }

        #region Profile Services

        public async Task<ProfileModel?> GetProfileAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var filter = Builders<ProfileModel>.Filter.Eq(i => i.Profile.UserId, userId);
            return await _profiles.Find(filter).FirstOrDefaultAsync();
        }

        public async Task InsertProfileAsync(ProfileModel profile) =>
            await _profiles.InsertOneAsync(profile);

        public async Task<bool> IsUsernameInUse(string? username)
        {
            if (string.IsNullOrEmpty(username))
                return false;

            // Use a case-insensitive regex filter
            var filter = Builders<ProfileModel>.Filter.Regex(p => p.Profile.Username, new MongoDB.Bson.BsonRegularExpression($"^{username}$", "i"));
            var existingProfile = await _profiles.Find(filter).FirstOrDefaultAsync();
            return existingProfile != null;
        }


        /// <summary>
        /// Sets a unique username for the profile if it hasn't been set already.
        /// </summary>
        public async Task<SetUsernameResponse> SetUsernameAsync(string userId, string username)
        {
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Profile.UserId, userId);
            var profile = await _profiles.Find(filter).FirstOrDefaultAsync();

            if (profile == null)
                throw new Exception("Profile not found.");

            if (profile.Profile.RemainingUsernameChanges <= 0)
                throw new InvalidOperationException("No remaining username changes allowed.");

            if (await IsUsernameInUse(username))
                throw new InvalidOperationException("Username is already in use.");

            // Update username and decrement RemainingUsernameChanges
            var update = Builders<ProfileModel>.Update
                .Set(p => p.Profile.Username, username)
                .Inc(p => p.Profile.RemainingUsernameChanges, -1);

            var result = await _profiles.UpdateOneAsync(filter, update);

            // If the update is successful, return the response model
            if (result.ModifiedCount > 0)
            {
                return new SetUsernameResponse
                {
                    Success = true,
                    RemainingUsernameChanges = profile.Profile.RemainingUsernameChanges - 1
                };
            }

            // If the update fails, return a failure response
            return new SetUsernameResponse
            {
                Success = false,
                RemainingUsernameChanges = profile.Profile.RemainingUsernameChanges
            };
        }

        public async Task<bool> UpdateProfilePictureAsync(string userId, int profilePictureId)
        {
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Profile.UserId, userId);
            var update = Builders<ProfileModel>.Update.Set("Profile.ProfilePictureId", profilePictureId);
            var result = await _profiles.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateBannerPictureAsync(string userId, int bannerPictureId)
        {
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Profile.UserId, userId);
            var update = Builders<ProfileModel>.Update.Set("Profile.BannerPictureId", bannerPictureId);
            var result = await _profiles.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        #endregion

        #region Currency Management

        public async Task<int> AddCurrencyAsync(string userId, int amount, bool isPremium = false)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be a positive integer.");

            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Profile.UserId, userId);
            var update = isPremium
                ? Builders<ProfileModel>.Update.Inc(p => p.Profile.PremiumCurrency, amount)
                : Builders<ProfileModel>.Update.Inc(p => p.Profile.Currency, amount);

            var result = await _profiles.FindOneAndUpdateAsync(
                filter,
                update,
                new FindOneAndUpdateOptions<ProfileModel>
                {
                    ReturnDocument = ReturnDocument.After
                });

            return isPremium ? result.Profile.PremiumCurrency : result.Profile.Currency;
        }

        public async Task<int> ConsumeCurrencyAsync(string userId, int amount, bool isPremium)
        {
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Profile.UserId, userId) &
                         Builders<ProfileModel>.Filter.Gte(isPremium ? p => p.Profile.PremiumCurrency : p => p.Profile.Currency, amount);

            var update = isPremium
                ? Builders<ProfileModel>.Update.Inc(p => p.Profile.PremiumCurrency, -amount)
                : Builders<ProfileModel>.Update.Inc(p => p.Profile.Currency, -amount);

            var result = await _profiles.FindOneAndUpdateAsync(
                filter,
                update,
                new FindOneAndUpdateOptions<ProfileModel>
                {
                    ReturnDocument = ReturnDocument.After
                });

            return isPremium ? result.Profile.PremiumCurrency : result.Profile.Currency;
        }

        #endregion
    }
}
