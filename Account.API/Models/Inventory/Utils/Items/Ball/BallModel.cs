namespace Account.API.Models.Inventory.Utils.Items.Ball
{
    public class BallModel
    {
        public string ballName { get; set; } = null!;
        public string description { get; set; } = null!;
        public int amount { get; set; }
        public Dictionary<string, object> data { get; set; } = null!;
    }
}
