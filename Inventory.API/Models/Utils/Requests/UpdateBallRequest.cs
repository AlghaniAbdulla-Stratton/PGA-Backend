namespace Inventory.API.Models.Utils.Requests
{
    public class UpdateBallRequest
    {
        public string inventoryId { get; set; } = null!;
        public string userId { get; set; } = null!;
        public string ballName { get; set; } = null!; // Change this to item ID once item bank is finished
        public int amount { get; set; }
    }
}
