using NFT.API.Models.Clubs.Utils;
using NFT.API.Models.Clubs.Interface;

namespace NFT.API.Models.Clubs
{
    public class ClubDataModel
    {
        public string brandId { get; set; } = null!;
        public HeadData headData { get; set; } = null!;
        public ShaftData shaftData { get; set; } = null!;
        public GripData gripData { get; set; } = null!;

        public ClubDataModel(HeadData headData, ShaftData shaftData, GripData gripData, string brandId)
        { 
            this.headData = headData;
            this.shaftData = shaftData;
            this.gripData = gripData;
            this.brandId = brandId;
        }
    }
}
