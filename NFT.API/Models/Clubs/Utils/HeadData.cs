using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NFT.API.Models.Clubs.Interface;

namespace NFT.API.Models.Clubs.Utils
{
    public class HeadData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = null!;
        public string brandId { get; set; } = null!;
        public int power { get; set; }
        public int control { get; set; }
        public int pitch { get; set; }
    }
}
