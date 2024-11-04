namespace Account.API.Models.Profile.Utils
{
    public class ConsumeCurrencyRequest
    {
        public string UserId { get; set; } = "";  // The user ID associated with this request

        public int Amount { get; set; }  // The amount of currency to be consumed

        public bool IsPremium { get; set; }  // Indicates if the currency is premium or regular
    }
}
