using ChineseAuction.Dtos;
using ChineseAuction.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly ILogger<PackageController> _logger;
        public PackageController(IPackageService packageService,ILogger<PackageController> logger)
        {
            _packageService = packageService;
            _logger = logger;
        }

        // get all packages
        [HttpGet]
        public async Task<IActionResult> GetAllPackages()
        {
            _logger.LogInformation("Starting to get all packages");
            var packages = await _packageService.GetAllPackagesAsync();
            _logger.LogInformation("Got all packages");
            return Ok(packages);
        }

        // get package by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            _logger.LogInformation("Starting to get package by id: {Id}", id);
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package == null) return NotFound("The id:" + id + " ,did not found🤚");
            _logger.LogInformation("Got package by id: {Id}", id);
            return Ok(package);
        }

        // add new package
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AddPackage([FromBody] CreatePackageDto createPackageDto)
        {
            _logger.LogInformation("Starting to add a new package");
            try
            {
                GetPackageDto package = await _packageService.AddPackageAsync(createPackageDto);
                _logger.LogInformation("Added a new package with id: {Id}", package.Id);
                return CreatedAtAction(nameof(GetPackageById), new { id = package.Id }, package);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex ,"Error occurred while adding a new package");
                return BadRequest("Internal server error occurred.");
            }
        }
        // update package
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromBody] CreatePackageDto updatePackageDto)
        {
            _logger.LogInformation("Starting to update package with id: {Id}", id);
            try
            {
                var updatedPackage = await _packageService.UpdatePackageAsync(id, updatePackageDto);
                if (updatedPackage == null) return NotFound("The id:" + id + " ,did not found🤚");
                _logger.LogInformation("Updated package with id: {Id}", id);
                return Ok(updatedPackage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating package with id: {Id}", id);
                return BadRequest(ex.Message);
            }
        }
        // delete package
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            _logger.LogInformation("Starting to delete package with id: {Id}", id);
            try
            {
                var isDeleted = await _packageService.DeletePackageAsync(id);
                if (!isDeleted) return NotFound("The id:" + id + " ,did not found🤚");
                _logger.LogInformation("Deleted package with id: {Id}", id);
                return Ok("Sucsses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting package with id: {Id}", id);
                return BadRequest("Internal server error occurred.");
            }
        }
    }
}