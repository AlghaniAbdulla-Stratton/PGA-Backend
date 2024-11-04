using Microsoft.AspNetCore.Mvc;
using Inventory.API.Services;
using Inventory.API.Models;
using Inventory.API.Models.Utils.Requests;
using Inventory.API.Models.Utils.Items.Ball;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class InventoryController : Controller
    {
        private readonly InventoryService _inventory;

        public InventoryController(InventoryService inventoryService) => _inventory = inventoryService;

        #region Inventory Accessor
        [HttpPost("CreateInventory/{userId}")]
        public async Task<string> CreateInventory(string userId)
        {
            var existing = await _inventory.CheckExistingInventory(userId);
            if(existing is null)
                return await _inventory.CreateInventory(userId);

            return existing.id;
        }

        [HttpGet("GetInventory")]
        public async Task<IActionResult> GetInventory([FromBody]InventoryValidation request)
        {
            var inventory = await _inventory.GetInventory(request);
            if (inventory is null)
                return NotFound("Can't find Inventory");

            return Ok(inventory);
        }
        #endregion

        #region Ball Methods
        [HttpPost("ConsumeBall")]
        public async Task<InventoryModel?> ConsumeBall([FromBody] UpdateBallRequest request)
        {
            return await _inventory.UpdateBall(request, true);
        }
        [HttpPost("AddBall")]
        public async Task<InventoryModel> AddBall([FromBody] UpdateBallRequest request)
        {
            return await _inventory.UpdateBall(request, false);
        }
        [HttpGet("RetrieveBall")]
        public async Task<IActionResult> GetBall([FromBody] GetBallRequest request)
        {
            var ball = await _inventory.GetBall(request);
            if (ball is null)
                return NotFound("Ball does not exist");

            return Ok(ball);
        }
        [HttpGet("RetrieveAllBalls")]
        public async Task<List<BallModel>> GetAllBalls([FromBody] InventoryValidation request)
        {
            return await _inventory.GetAllBalls(request);
        }
        #endregion
    }
}
