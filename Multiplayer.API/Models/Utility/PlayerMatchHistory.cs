
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PGALegends.Models
{
    public class PlayerMatchHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string UserId { get; set; } = null!;
        public string[] PlayedMatchIds { get; set; } = null!;
    }
}
