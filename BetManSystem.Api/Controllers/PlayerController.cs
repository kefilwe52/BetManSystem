using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Common.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BetManSystem.Api.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(
            IPlayerService playerService,
            ILogger<PlayerController> logger)
        {
            _playerService = playerService;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Player), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var player = await _playerService.GetByIdAsync(id);
                if (player == null)
                    return NotFound();
                return Ok(player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetById for Player {PlayerId}", id);
                return StatusCode(500, "An error occurred while fetching the player.");
            }
        }

        [HttpGet("by-email")]
        [ProducesResponseType(typeof(Player), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            try
            {
                var player = await _playerService.GetByEmailAsync(email);
                if (player == null)
                    return NotFound();
                return Ok(player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByEmail for {Email}", email);
                return StatusCode(500, "An error occurred while fetching the player.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Player), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Player player)
        {
            try
            {
                var created = await _playerService.CreateAsync(player);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Player {@Player}", player);
                return BadRequest("Failed to create player.");
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Player), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] Player player)
        {
            if (id != player.Id)
                return BadRequest("Id mismatch.");

            try
            {
                var updated = await _playerService.UpdateAsync(player);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Player {@Player}", player);
                return BadRequest("Failed to update player.");
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _playerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Player {PlayerId}", id);
                return BadRequest("Failed to delete player.");
            }
        }
    }
}
