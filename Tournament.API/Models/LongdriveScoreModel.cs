using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tournament.API.Models
{
    public class LongdriveScoreModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; } = null!;
        public string ownerId { get; set; } = null!;
        public int score { get; set; }
        public string region { get; set; } = null!;
        public int timestamp { get; set; }
    }
}
