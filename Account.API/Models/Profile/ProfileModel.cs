using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Account.API.Models.Profile.Utils;

namespace Account.API.Models.Profile
{
    public class ProfileModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public ProfileData Profile { get; set; } = null!;
        public ExternalReference ExternalReference { get; set; } = null!;
    }
}
