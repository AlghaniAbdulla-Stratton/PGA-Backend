namespace Inventory.API.Models.Utils.Items.Ball
{
    public class BallModel
    {
        public string itemId { get; set; } = null!;
        public string ballName { get; set; } = null!;
        public string description { get; set; } = null!;
        public int amount { get; set; }
        public Dictionary<string, object> data { get; set; } = new Dictionary<string, object>();
    }
}
