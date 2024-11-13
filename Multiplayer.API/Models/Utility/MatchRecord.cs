
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PGALegends.Models
{
    /// <summary>
    /// The base class that contains the metadata for a match record.
    /// </summary>
    [Serializable]
    public class MatchRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        /// <summary>
        /// Unique identifiers for the two players in the match.
        /// </summary>
        public string Player1Id { get; set; } = string.Empty;
        public string Player2Id { get; set; } = string.Empty;

        /// <summary>
        /// MongoDB IDs referencing the recorded match data for each player.
        /// </summary>
        public string Player1MatchDataId { get; set; } = string.Empty;
        public string Player2MatchDataId { get; set; } = string.Empty;

        /// <summary>
        /// The game type being played (e.g., Long Drive).
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the map where the match took place.
        /// </summary>
        public int MapId { get; set; }

        // Private serialized field for the ISO 8601 date string
        public string MatchDateTimeString { get; set; } = string.Empty;

        /// <summary>
        /// The index indicating the winning player (0 for Player1, 1 for Player2).
        /// </summary>
        public int WinnerIndex { get; set; }

        /// <summary>
        /// Arrays representing scores for each turn for Player1 and Player2.
        /// </summary>
        public int[] Player1Scores { get; set; } = Array.Empty<int>();
        public int[] Player2Scores { get; set; } = Array.Empty<int>();
    }

}
