using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Item.API.Models.Clubs.Utils;

namespace Item.API.Models.Clubs
{
    public class Scaling
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string scaleTag { get; set; } = null!;
        public Row[] rows { get; set; } = null!;
    }
}
