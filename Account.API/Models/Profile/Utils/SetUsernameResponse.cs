namespace Account.API.Models.Profile.Utils
{
    public class SetUsernameResponse
    {
        public bool Success { get; set; }
        public int RemainingUsernameChanges { get; set; }
    }
}