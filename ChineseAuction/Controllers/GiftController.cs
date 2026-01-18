using ChineseAuction.Dtos;
using ChineseAuction.Extensions;
using ChineseAuction.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChineseAuction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;
        private readonly ILogger<GiftController> _logger;
        public GiftController(IGiftService giftService,ILogger<GiftController> logger)
        {
            _giftService = giftService;
            _logger = logger;
        }

        // Get all approved gifts
        [HttpGet]
        public async Task<IActionResult> GetAllApprovedGiftsAsync()
        { 
            _logger.LogInformation("Starting to get all approved gifts");
            var gifts =await _giftService.GetAllApprovedGiftsAsync();
            _logger.LogInformation("Got all approved gifts");
            return Ok(gifts);
        }

        // Get all unapproved gifts
        [Authorize(Roles = "Manager")]
        [HttpGet("Unapproved")]
        public async Task<IActionResult> GetNoneUnapprovedGifts()
        {
            _logger.LogInformation("Starting to get all unapproved gifts");
            var gifts =await _giftService.GetAllUnapprovedGiftsAsync();
            _logger.LogInformation("Got all unapproved gifts");
            return Ok(gifts);

        }

        // get gift by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGiftById(int id)
        {
            _logger.LogInformation("Starting to get gift by id: {GiftId}", id);
            var gift =await _giftService.GetGiftByIdAsync(id);
            _logger.LogInformation("Got gift by id: {GiftId}", id);
            if (gift == null) return NotFound("The id:" + id + " ,did not found🤚");
            return Ok(gift);
        }

        // Add new gift
        [Authorize(Roles = "Manager")]//donor
        [HttpPost]
        public async Task<IActionResult> AddGift([FromBody] CreateGiftDto giftDto)
        {
            _logger.LogInformation("Starting to add a new gift");
            try
            {
                var createdGift =await _giftService.AddGiftAsync(giftDto);
                _logger.LogInformation("Added a new gift with id: {GiftId}", createdGift.Id);
                return CreatedAtAction(nameof(GetGiftById), new { id = createdGift.Id }, createdGift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new gift");
                return BadRequest(ex.Message);
            }
        }

        // Update gift
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGift(int id, [FromBody] CreateGiftDto giftDto)
        {
            _logger.LogInformation("Starting to update gift with id: {GiftId}", id);
            try
            {
                var updatedGift =await _giftService.UpdateGiftAsync(id, giftDto);
                if (updatedGift == null) return NotFound("The id:" + id + " ,did not found🤚");
                _logger.LogInformation("Updated gift with id: {GiftId}", id);
                return Ok(updatedGift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating gift with id: {GiftId}", id);
                return BadRequest(ex.Message);
            }
        }

        // Update gift purchases quantity
        [Authorize]
        [HttpPut("purchase/{giftId}")]
        public async Task<IActionResult> UpdateGiftPurchasesQuantity(int giftId)
        {
            _logger.LogInformation("Starting to update purchases quantity for gift with id: {GiftId}", giftId);
            try
            {
                var updatedGift =await _giftService.UpdateGiftPurchasesQuantityAsync(giftId);
                if (updatedGift == null) return NotFound("The id:" + giftId + " ,did not found🤚");
                _logger.LogInformation("Updated purchases quantity for gift with id: {GiftId}", giftId);
                return Ok(updatedGift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating purchases quantity for gift with id: {GiftId}", giftId);
                return BadRequest(ex.Message);
            }
        }

        // Delete gift
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            _logger.LogInformation("Starting to delete gift with id: {GiftId}", id);
            try
            {
                var result =await _giftService.DeleteGiftAsync(id);
                if (!result) return NotFound("The id:" + id + " ,did not found🤚");
                _logger.LogInformation("Deleted gift with id: {GiftId}", id);
                return Ok("Gift with id:" + id + " has been deleted successfully🗑️");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting gift with id: {GiftId}", id);
                return BadRequest(ex.Message);
            }
        }
        // Update approval status
        [Authorize(Roles = "Manager")]
        [HttpPut("approve")]
        public async Task<IActionResult> UpdateApprovalStatus([FromBody] ApproveGiftDto gift)
        {
            _logger.LogInformation("Starting to update approval status for gift with id: {GiftId}", gift.Id);
            try
            {
                var result =await _giftService.UpdateApprovalStatusAsync(gift);
                if (!result) return NotFound("The id:" + gift.Id + " ,did not found🤚");
                _logger.LogInformation("Updated approval status for gift with id: {GiftId}", gift.Id);
                return Ok("Gift with id:" + gift.Id + " approval status has been updated successfully✅");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating approval status for gift with id: {GiftId}", gift.Id);
                return BadRequest(ex.Message);
            }
        }

        // Get filtered gifts
        [HttpGet("search")]
        public async Task<IActionResult> GetFilteredGifts([FromQuery] string? giftName, [FromQuery] string? donorName, [FromQuery] int? minPurchases)
        {
            _logger.LogInformation("Starting to search gifts. GiftName: {GiftName}, Donor: {DonorName}", giftName, donorName);
            try
            {
                var gifts = await _giftService.GetFilteredGiftsAsync(giftName, donorName, minPurchases);
                _logger.LogInformation("Successfully retrieved filtered gifts.");
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching gifts.");
                return BadRequest(ex.Message);
            }
        }

        // Get sorted purchases
        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedPurchases([FromQuery] string sortBy = "popularity")
        {
            _logger.LogInformation("Starting to get sorted purchases by: {SortBy}", sortBy);
            try
            {
                var purchases = await _giftService.GetSortedGiftAsync(sortBy);
                _logger.LogInformation("Successfully retrieved sorted purchases.");
                return Ok(purchases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting sorted purchases.");
                return BadRequest("Internal server error occurred");
            }
        }
    }
}