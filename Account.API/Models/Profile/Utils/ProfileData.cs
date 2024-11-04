namespace Account.API.Models.Profile.Utils
{
    public class ProfileData
    {
        public string? UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ProfilePictureId { get; set; } = null!;
        public string BannerPictureId { get; set; } = null!;
        public string Club { get; set; } = null!;
        public int Currency { get; set; }
        public int PremiumCurrency { get; set; }
        public int RemainingUsernameChanges { get; set; }
        public string? GooglePlayId { get; set; } = null!;
        public string? AppleGameCenterId { get; set; } = null!;
        public string? FacebookId { get; set; } = null!;
    }
}
