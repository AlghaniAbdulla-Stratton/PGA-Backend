namespace Account.API.Models.Profile.Utils
{
    public class SetBannerPictureRequest
    {
        public string? UserId { get; set; } = null!;
        public int BannerPictureId { get; set; }
    }
}