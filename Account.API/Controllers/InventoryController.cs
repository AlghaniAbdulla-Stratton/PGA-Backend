using Account.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Account.API.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class InventoryController : Controller
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService) => _inventoryService = inventoryService;


    }
}
