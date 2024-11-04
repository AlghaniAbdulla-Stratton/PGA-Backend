namespace Account.API.Models.Profile.Utils
{
    public class SetProfilePictureRequest
    {
        public string? UserId { get; set; } = null!;
        public int ProfilePictureId { get; set; }
    }
}