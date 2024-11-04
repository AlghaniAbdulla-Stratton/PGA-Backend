namespace Account.API.Models.Profile.Utils
{
    public class AddCurrencyRequest
    {
        public string UserId { get; set; } = "";
        public int Amount { get; set; } // Positive integer for currency to add
        public bool IsPremium { get; set; } // True for premium currency, false for regular
    }
}