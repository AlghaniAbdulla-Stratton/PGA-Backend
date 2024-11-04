namespace Account.API.Models.Profile.Utils
{
    public class GetProfileRequest
    {
        public string UserId { get; set; }

        public GetProfileRequest(string userId)
        {
            UserId = userId;
        }
    }
}
