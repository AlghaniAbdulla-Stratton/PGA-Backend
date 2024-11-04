using Microsoft.AspNetCore.Mvc;
using NFT.API.Models.Clubs;
using NFT.API.Services;

namespace NFT.API.Controllers
{
    [ApiController]
    [Route("/api/v1/[Controller]/")]
    public class ClubController : Controller
    {
        private readonly NFTService _service;

        public ClubController(NFTService NftService) => _service = NftService;

        [HttpPost("CraftClub")]
        public async Task<IActionResult> CraftClub([FromBody]ClubPartsModel? clubParts, [FromQuery]string? headId, [FromQuery]string? shaftId, [FromQuery]string? gripId)
        {
            if (headId == null && clubParts == null) return BadRequest(new { message = "Head ID is required" });
            if (shaftId == null && clubParts == null) return BadRequest(new { message = "Shaft ID is required" });
            if (gripId == null && clubParts == null) return BadRequest(new { message = "Grip ID is required" });

            if (clubParts == null && (!string.IsNullOrWhiteSpace(headId) || !string.IsNullOrWhiteSpace(shaftId) || !string.IsNullOrWhiteSpace(gripId)))
                clubParts = new ClubPartsModel
                {
                    HeadId = headId,
                    ShaftId = shaftId,
                    GripId = gripId
                };


            var head = await _service.GetHeadDataByIdAsync(clubParts.HeadId);
            var shaft = await _service.GetShaftDataByIdAsync(clubParts.ShaftId);
            var grip = await _service.GetGripDataByIdAsync(clubParts.GripId);

            if (head is null) return NotFound(new {message = $"No head part found with ID: {clubParts.HeadId}" });
            if (shaft is null) return NotFound(new {message = $"No shaft part found with ID: {clubParts.ShaftId}" });
            if (grip is null) return NotFound(new {message = $"No grip part found with ID: {clubParts.GripId}" });

            if (!head.brandId.Equals(shaft.brandId) || !shaft.brandId.Equals(grip.brandId) || !grip.brandId.Equals(head.brandId))
                return BadRequest(new { message = "Head, Shaft, and Grip must be of the same Brand" });

            var clubData = new ClubDataModel(head,shaft,grip,head.brandId);

            return Ok(clubData);
        }
    }
}
