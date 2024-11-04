using Inventory.API.Models;
using Inventory.API.Models.Utils.Items;
using Inventory.API.Models.Utils.Items.Ball;
using Inventory.API.Models.Utils.Requests;
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

        #region Inventory Accessor 
        public async Task<string> CreateInventory(string userId)
        {
            var newInventory = new InventoryModel
            {
                ownerId = userId,
                content = new ContentModel
                {
                    balls = new List<BallModel>()
                }
            };

            await _inventory.InsertOneAsync(newInventory);

            return newInventory.id.ToString();
        }
        public async Task<InventoryModel> GetInventory(InventoryValidation request)
        {
            var inventory = await _inventory.Find(i => i.id == request.inventoryId).FirstOrDefaultAsync();
            if (inventory is null)
                throw new Exception("Inventory not found");

            if (inventory.ownerId != request.userId)
                throw new Exception("User does not own this inventory!");

            return inventory;
        }
        public async Task<InventoryModel?> CheckExistingInventory(string userId)
        {
            var filter = Builders<InventoryModel>.Filter.Eq(i => i.ownerId, userId);
            var count = await _inventory.CountDocumentsAsync(filter);
            if (count is 0)
                return null;

            return await _inventory.Find(filter).FirstOrDefaultAsync();
        }
        #endregion

        #region Ball Methods
        // EACH ITEM SHOULD HAVE THEIR OWN ITEM ID IN THE ITEM BANK (CMS)
        // BEST OPTION TO PREVENT AMBIGUITY IS TO USE ITEM ID INSTEAD OF ITEM NAME
        public async Task<InventoryModel> UpdateBall(UpdateBallRequest request, bool consume)
        {
            var filter = Builders<InventoryModel>.Filter.Eq(i => i.id, request.inventoryId);
            var inventory = await _inventory.Find(filter).FirstOrDefaultAsync();

            if (inventory is null)
                throw new Exception("Inventory not found");

            if (inventory.ownerId != request.userId)
                throw new Exception("User does not own this inventory!");

            inventory.content ??= new ContentModel();
            inventory.content.balls ??= new List<BallModel>();

            var existingBall = inventory.content.balls.FirstOrDefault(b => b.ballName.Equals(request.ballName));

            if (existingBall is null)
                inventory.content.balls.Add(new BallModel
                {
                    ballName = request.ballName,
                    amount = request.amount
                });
            else
            {
                if (consume)
                {
                    existingBall.amount = Math.Max(0, existingBall.amount - request.amount);

                    if (existingBall.amount == 0)
                        inventory.content.balls.Remove(existingBall);
                }
                else
                    existingBall.amount += request.amount;
            }
            var updatedInventory = Builders<InventoryModel>.Update.Set(i => i.content.balls, inventory.content.balls);
            await _inventory.UpdateOneAsync(filter, updatedInventory);
            return await _inventory.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<BallModel?> GetBall(GetBallRequest request)
        {
            var inventory = await _inventory.Find(i => i.id == request.inventoryId).FirstOrDefaultAsync();

            if (inventory is null)
                throw new Exception("Inventory not found");

            if (inventory.ownerId != request.userId)
                throw new Exception("User does not own this inventory!");

            var ball = inventory.content.balls.FirstOrDefault(b => b.ballName.Equals(request.ballName));

            if (ball is null)
                return null;

            return ball;
        }
        public async Task<List<BallModel>> GetAllBalls(InventoryValidation request)
        {
            var inventory = await _inventory.Find(i => i.id == request.inventoryId).FirstOrDefaultAsync();

            if (inventory is null)
                throw new Exception("Inventory not found");

            if (inventory.ownerId != request.userId)
                throw new Exception("User does not own this inventory!");

            if (inventory.content.balls.Count() == 0)
                return new List<BallModel>();
            return inventory.content.balls;
        }
        #endregion
    }
}
