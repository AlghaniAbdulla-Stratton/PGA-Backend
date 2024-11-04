using Microsoft.AspNetCore.Mvc;
using Inventory.API.Services;

namespace Inventory.API.Controllers
{

    [ApiController]
    [Route("/api/v1/[controller]")]
    public class InventoryController : Controller
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService) => _inventoryService = inventoryService;


    }
}
