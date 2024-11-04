namespace Multiplayer.API.Models.Utility
{
    public class PlayerTurnsData
    {
        public int RngSeed { get; set; }
        public string[] TakeThatsAppliedToSelf { get; set; } = null!;
        public string[] TakeThatsToApplyToOpponent { get; set; } = null!;
        public RecordedStrike RecordedStrike { get; set; } = null!;
    }
}
