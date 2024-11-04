using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Social.API.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Sender { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
