namespace Inventory.API.Models.Utils.Requests
{
    public class GetBallRequest
    {
        public string inventoryId { get; set; } = null!;
        public string userId { get; set; } = null!;
        public string ballName { get; set; } = null!;
    }
}
