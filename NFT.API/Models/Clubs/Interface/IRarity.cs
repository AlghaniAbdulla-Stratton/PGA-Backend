namespace NFT.API.Models.Clubs.Interface
{
    public interface IRarity
    {
        public Rarity GetRarity();
    }

    public enum Rarity
    {
        Entry = 0, // 300/600, 50%
        Intermediate = 1, // 375/600, 62.5%
        Advanced = 2, // 450/600, 75%
        Master = 3, // 525/600, 87.5%
        Champion = 4, // 600/600 100%
    }
}
