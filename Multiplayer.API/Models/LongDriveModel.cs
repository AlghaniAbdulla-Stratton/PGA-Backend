using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Multiplayer.API.Models.Utility;

namespace Multiplayer.API.Models
{
    public class LongDriveModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string UserId { get; set; } = null!;
        public int MapId { get; set; }
        public PlayerTurnsData[] PlayerTurnsData { get; set; } = null!;
    }
}
