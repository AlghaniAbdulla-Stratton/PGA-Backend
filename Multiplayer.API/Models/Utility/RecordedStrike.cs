namespace Multiplayer.API.Models.Utility
{
    public class RecordedStrike
    {
        public int FrameInterval { get; set; }
        public int TotalFrameCount { get; set; }
        public SwingData SwingData { get; set; } = null!;
        public StrikeData StrikeData { get; set; } = null!;
        public StateData[] States { get; set; } = null!;
    }
}
