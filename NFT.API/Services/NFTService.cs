using NFT.API.Models.Clubs.Interface;
using NFT.API.Models.Clubs.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NFT.API.Models;

namespace NFT.API.Services
{
    public class NFTService
    {
        private readonly IMongoCollection<HeadData> _heads;
        private readonly IMongoCollection<ShaftData> _shafts;
        private readonly IMongoCollection<GripData> _grips;

        private Rarity rarity;

        public NFTService(IOptions<NFTDatabaseSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionName);
            var itemBank = mongoClient.GetDatabase(options.Value.ItemBankDatabase);
            _heads = itemBank.GetCollection<HeadData>(options.Value.ClubHeadCollection);
            _shafts = itemBank.GetCollection<ShaftData>(options.Value.ClubShaftCollection);
            _grips = itemBank.GetCollection<GripData>(options.Value.ClubGripCollection);
        }

        #region Part Getter
        public async Task<HeadData> GetHeadDataByIdAsync(string id) => 
            await _heads.Find(i => i.id == id).FirstOrDefaultAsync();
        public async Task<ShaftData> GetShaftDataByIdAsync(string id) =>
            await _shafts.Find(i => i.id == id).FirstOrDefaultAsync();
        public async Task<GripData> GetGripDataByIdAsync(string id) =>
            await _grips.Find(i => i.id == id).FirstOrDefaultAsync();
        #endregion
    }
}
