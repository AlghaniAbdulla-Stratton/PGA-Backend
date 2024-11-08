using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Tournament.API.Models;
using Tournament.API.Services;

namespace Tournament.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LeaderboardController : Controller
    {
        private readonly LeaderboardService _service;

        public LeaderboardController(LeaderboardService service) => _service = service;

        #region Longdrive Leaderboards
        [HttpGet("GetLongdriveEntry/{userId}")]
        public async Task<LongdriveScoreModel?> GetLongdriveEntry(string userId)
        {
            var result = await _service.GetLongdriveEntry(userId);
            if (result is null)
                return null;
            return result;
        }
        [HttpGet("GetLongdriveBoard/{page}")]
        public async Task<List<LongdriveScoreModel>> GetLongdriveBoard(int page)
        {
            var leaderboard = await _service.GetLongdriveBoard(page);

            return leaderboard;
        }
        [HttpPost("LongdriveEntry")]
        public async Task<IActionResult> LongdriveEntry([FromBody]LongdriveScoreModel scoreModel)
        {
            try
            {
                await _service.CreateOrUpdateEntry(scoreModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
        #endregion
    }
}
