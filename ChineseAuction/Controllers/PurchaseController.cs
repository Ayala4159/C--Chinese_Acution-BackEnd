using ChineseAuction.Dtos;
using ChineseAuction.Extensions;
using ChineseAuction.Models;
using ChineseAuction.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chinese_Auction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(IPurchaseService purchaseService, ILogger<PurchaseController> logger)
        {
            _purchaseService = purchaseService;
            _logger = logger;
        }

        // get all purchases
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetAllPurchases()
        {
            _logger.LogInformation("Starting to get all purchases");
            var purchases = await _purchaseService.GetAllPurchasesAsync();
            _logger.LogInformation("Got getting all purchases succesfully");
            return Ok(purchases);
        }

        // get purchase by id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchaseById(int id)
        {
            _logger.LogInformation("Starting to get purchase by id: {Id}", id);
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id);
            _logger.LogInformation("Got purchase by id: {Id} succesfully", id);
            return purchase == null ? NotFound() : Ok(purchase);
        }

        // add purchases
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPurchases([FromBody] List<CreatePurchaseDto> purchaseDtos)
        {

            _logger.LogInformation("Starting to add purchases");
            try
            {
                if (purchaseDtos == null || !purchaseDtos.Any()) return BadRequest("Purchase list is empty");
                var createdPurchases = await _purchaseService.AddPurchaseAsync(purchaseDtos);
                _logger.LogInformation("Added purchases succesfully");
                return CreatedAtAction(nameof(GetAllPurchases), createdPurchases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding purchases");
                return BadRequest("Internal server error occurred");
            }

        }

        // update purchase
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchase(int id, [FromBody] UpdatePurchaseDto purchaseDto)
        {
            if (!User.IsManager() && User.GetUserId() != id)
            {
                return Forbid();
            }
            _logger.LogInformation("Starting to update purchase with id: {Id}", id);
            try
            {
                var updated = await _purchaseService.UpdatePurchaseAsync(id, purchaseDto);
                _logger.LogInformation("Updated purchase with id: {Id} succesfully", id);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating purchase with id: {Id}", id);
                return BadRequest("Internal server error occurred");
            }

        }

        // get purchases by user id
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            if (!User.IsManager() && User.GetUserId() != userId)
            {
                return Forbid();
            }
            _logger.LogInformation("Starting to get purchases by user id: {UserId}", userId);
            var purchases = await _purchaseService.GetPurchasesByUserIdAsync(userId);
            _logger.LogInformation("Got purchases by user id: {UserId} succesfully", userId);
            return Ok(purchases);
        }

        // get purchases by gift id 
        [Authorize(Roles = "Manager")]
        [HttpGet("gift/{giftId}")]
        public async Task<IActionResult> GetByGiftId(int giftId)
        {
            _logger.LogInformation("Starting to get purchases by gift id: {GiftId}", giftId);
            var purchases = await _purchaseService.GetPurchasesByGiftIdAsync(giftId);
            _logger.LogInformation("Got purchases by gift id: {GiftId} succesfully", giftId);
            return Ok(purchases);
        }

        // run lottery for a specific gift
        //[Authorize(Roles = "Manager")]
        [HttpPost("lottery/{giftId}")]
        public async Task<IActionResult> RunLottery(int giftId)
        {
            _logger.LogInformation("Starting lottery for gift id: {GiftId}", giftId);
            var winner = await _purchaseService.Lottory(giftId);
            if (winner == null) return BadRequest("No participants for this gift or lottery failed.");
            _logger.LogInformation("Lottery for gift id: {GiftId} completed successfully", giftId);
            return Ok(winner);
        }


        // get winner by gift id
        [HttpGet("winner/{giftId}")]
        public async Task<IActionResult> GetWinner(int giftId)
        {
            _logger.LogInformation("Starting to get winner for gift id: {GiftId}", giftId);
            var winner = await _purchaseService.GetWinnersByGiftIdAsync(giftId);
            if (winner == null) return NotFound("No winner found for this gift yet.");
            _logger.LogInformation("Got winner for gift id: {GiftId} successfully", giftId);
            return Ok(winner);
        }

        // Get sorted purchases for manager
        [Authorize(Roles = "Manager")]
        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedPurchases([FromQuery] string sortBy = "popularity")
        {
            _logger.LogInformation("Starting to get sorted purchases by: {SortBy}", sortBy);
            try
            {
                var purchases = await _purchaseService.GetSortedPurchasesAsync(sortBy);
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
