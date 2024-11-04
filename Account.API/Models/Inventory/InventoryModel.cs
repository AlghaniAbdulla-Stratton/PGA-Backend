using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Account.API.Models.Inventory.Utils.Items;

namespace Account.API.Models.Inventory
{
    public class InventoryModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = null!;
        public string ownerId { get;set; } = null!;
        public ContentModel content { get; set; } = null!;
    }
}
