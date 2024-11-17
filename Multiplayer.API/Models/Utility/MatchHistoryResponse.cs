
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PGALegends.Models
{
    /// <summary>
    /// The base class that contains the metadata for a match record.
    /// </summary>
    [Serializable]
    public class MatchHistoryResponse
    {
        public MatchRecord[] MatchRecords { get; set; } = Array.Empty<MatchRecord>();
    }

}
