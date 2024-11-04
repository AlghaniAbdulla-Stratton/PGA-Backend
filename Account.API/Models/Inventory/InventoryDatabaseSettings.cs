namespace Account.API.Models.Inventory
{
    public class InventoryDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string InventoryCollectionName { get; set; } = null!;
    }
}
