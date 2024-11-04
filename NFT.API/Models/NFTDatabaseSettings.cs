namespace NFT.API.Models
{
    public class NFTDatabaseSettings
    {
        public string ConnectionName { get; set; } = null!;
        public string ItemBankDatabase { get; set; } = null!;
        public string ClubScalingCollection { get; set; } = null!;
        public string ClubHeadCollection { get; set; } = null!;
        public string ClubShaftCollection { get; set; } = null!;
        public string ClubGripCollection { get; set; } = null!;
    }
}
