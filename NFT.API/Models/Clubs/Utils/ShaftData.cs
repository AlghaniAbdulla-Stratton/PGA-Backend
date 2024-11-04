using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NFT.API.Models.Clubs.Interface;

namespace NFT.API.Models.Clubs.Utils
{
    public class ShaftData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = null!;
        public string brandId { get; set; } = null!;
        public int accuracy { get; set; }
        public int tempo { get; set; }
        public int durability { get; set; }
    }
}
