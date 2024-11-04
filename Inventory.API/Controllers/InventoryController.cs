using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    public class InventoryController : Controller
    {
        [ApiController]
        [Route("/api/v1/[controller]")]
        public class InventoryController : Controller
        {
            private readonly InventoryService _inventoryService;

            public InventoryController(InventoryService inventoryService) => _inventoryService = inventoryService;


        }
    }
}
