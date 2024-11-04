using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Inventory.API.Models.Utils.Items;

namespace Inventory.API.Models
{
    public class InventoryModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = null!;
        public string ownerId { get; set; } = null!;
        public ContentModel content { get; set; } = null!;
    }
}
