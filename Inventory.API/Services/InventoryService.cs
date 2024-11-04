﻿using Inventory.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Inventory.API.Services
{
    public class InventoryService
    {
        public readonly IMongoCollection<InventoryModel> _inventory;

        public InventoryService(IOptions<InventoryDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _inventory = mongoDatabase.GetCollection<InventoryModel>(settings.Value.InventoryCollectionName);
        }

        #region Inventory Creation
        public Task<string> CreateProfileInventory(string userId)
        {
            return null;
        }
        #endregion
    }
}
