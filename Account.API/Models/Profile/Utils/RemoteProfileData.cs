namespace Account.API.Models.Profile.Utils
{
    // This model represents a remote profile data structure that can be retrieved when accessing another user's profile. 
    // It provides limited information to respect privacy, including basic identifiers and public display elements like 
    // username, profile picture, banner picture, and club affiliation. Used in scenarios where full profile details 
    public class RemoteProfileData
    {
        public string? UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string ProfilePictureId { get; set; } = null!;
        public string BannerPictureId { get; set; } = null!;
        public string Club { get; set; } = null!;
    }
}
