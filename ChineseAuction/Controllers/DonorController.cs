using ChineseAuction.Dtos;
using ChineseAuction.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;
        private readonly ILogger<DonorController> _logger;
        public DonorController(IDonorService donorService, ILogger<DonorController> logger)
        {
            _donorService = donorService;
            _logger = logger;
        }

        // Get all donors
        [Authorize(Roles = "manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllDonors()
        {
            _logger.LogInformation("Starting to get all donors...");
            var donors = await _donorService.GetAllDonorsAsync();
            _logger.LogInformation("Got all categories successfully");
            return Ok(donors);
        }

        // get donor by id
        [Authorize(Roles = "manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonorById(int id)
        {
            _logger.LogInformation("Starting to get donor by id: {Id}", id);
            var donor = await _donorService.GetDonorByIdAsync(id);
            _logger.LogInformation("Got donor by id: {Id} successfully", id);
            if (donor == null) return NotFound("The id:" + id + " ,did not found🤚");
            return Ok(donor);
        }

        // Add new donor
        [Authorize(Roles = "manager")]
        [HttpPost]
        public async Task<IActionResult> AddDonor([FromForm] CreateDonorDto createDonorDto, IFormFile? imageFile)
        {
            _logger.LogInformation("Starting to add a new donor...");
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/companies", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    createDonorDto.Company_picture = fileName;
                }
                var createdDonor = await _donorService.AddDonorAsync(createDonorDto);
                _logger.LogInformation("Added a new donor successfully with id: {Id}", createdDonor.Id);
                return CreatedAtAction(nameof(GetDonorById), new { id = createdDonor.Id }, createdDonor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new donor");
                return BadRequest(ex.Message);
            }
        }
        // Update donor
        [Authorize(Roles = "manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonor(int id, [FromForm] CreateDonorDto updateDonorDto, IFormFile? imageFile)
        {
            _logger.LogInformation("Starting to update donor with id: {Id}", id);
            try
            {
                var existingDonor = await _donorService.GetDonorByIdAsync(id);
                if (existingDonor == null) return NotFound();

                if (!string.IsNullOrEmpty(existingDonor.Company_picture))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/companies", existingDonor.Company_picture);
                    if (System.IO.File.Exists(oldFilePath) && imageFile != null) // מוחקים רק אם באמת מעלים תמונה חדשה
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/companies", fileName);
                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    updateDonorDto.Company_picture = fileName;
                }
                else
                {
                    updateDonorDto.Company_picture = existingDonor.Company_picture;
                }

                var updatedDonor = await _donorService.UpdateDonorAsync(id, updateDonorDto);
                if (updatedDonor == null) return NotFound("The id:" + id + " ,did not found🤚");

                return Ok(updatedDonor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating donor with id: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
        // Delete donor
        [Authorize(Roles = "manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            _logger.LogInformation("Starting to delete donor with id: {Id}", id);
            try
            {
                var result = await _donorService.DeleteDonorAsync(id);
                if (!result) return NotFound("The id:" + id + " ,did not found🤚");
                _logger.LogInformation("Deleted donor with id: {Id} successfully", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting donor with id: {Id}", id);
                return BadRequest(ex.Message);
            }

        }

        // Get filtered donors
        [Authorize(Roles = "manager")]
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredDonors([FromQuery] string? name, [FromQuery] string? email, [FromQuery] string? giftName)
        {
            _logger.LogInformation("Starting to get filtered donors. Name: {Name}, Email: {Email}, Gift: {Gift}", name, email, giftName);
            try
            {
                var donors = await _donorService.GetFilteredDonorsAsync(name, email, giftName);
                _logger.LogInformation("Successfully retrieved filtered donors.");
                return Ok(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filtering donors.");
                return BadRequest(ex.Message);
            }
        }
    }
}