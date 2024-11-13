
namespace PGALegends.Models
{
    public class PlayerMatchHistory
    {
        public string UserId { get; set; } = null!;
        public string[] PlayedMatchIds { get; set; } = null!;
    }
}
