using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NFT.API.Models.Clubs.Interface;

namespace NFT.API.Models.Clubs.Utils
{
    public class GripData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = null!;
        public string brandId { get; set; } = null!;
        public string takeThatId { get; set; } = null!;
    }
}
