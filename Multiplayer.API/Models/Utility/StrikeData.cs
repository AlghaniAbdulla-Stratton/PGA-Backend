namespace Multiplayer.API.Models.Utility
{
    public class StrikeData
    {
        public Coordinates StagedTarget { get; set; } = null!;
        public float Power { get; set; }
        public float Pitch { get; set; }
        public float Spin { get; set; }
        public float Tempo { get; set; }
    }
}
