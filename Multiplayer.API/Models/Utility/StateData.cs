namespace Multiplayer.API.Models.Utility
{
    public class StateData
    {
        public int State { get; set; }
        public Coordinates Position { get; set; } = null!;
        public Coordinates Rotation { get; set; } = null!;
        public Coordinates LinearVelocity { get; set; } = null!;
        public Coordinates AngularVelocity { get; set; } = null!;
        public float Drag { get; set; }
    }
}
