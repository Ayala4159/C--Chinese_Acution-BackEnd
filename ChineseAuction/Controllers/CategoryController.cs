using ChineseAuction.Dtos;
using ChineseAuction.Models;
using ChineseAuction.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            this._logger = logger;
        }

        // Get all categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            _logger.LogInformation("Starting to get all categories...");
            var categories = await _categoryService.GetAllCategoriesAsync();
            _logger.LogInformation("Got all categories successfully.");
            return Ok(categories);
        }

        // Get category by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            _logger.LogInformation("Starting to get category with id {Id}...", id);
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("The id:" + id + " ,did not found🤚");
            }
            _logger.LogInformation("Got category with id {Id} successfully.", id);
            return Ok(category);
        }

        // Add new category
        [Authorize(Roles = "manager")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] CategoryDto createCategoryDto, IFormFile imageFile)
        {
            _logger.LogInformation("Starting to add new category...");
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    return BadRequest("לא נבחרה תמונה");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                var newCategory = new CategoryDto { Name = createCategoryDto.Name, picture = fileName };
                GetCategoryDto category = await _categoryService.AddCategoryAsync(newCategory);
                _logger.LogInformation("Added new category successfully with id {Id}.", category.Id);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new category ");
                return BadRequest(ex.Message);
            }
        }
        // Update category
        [Authorize(Roles = "manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryDto updateDto, IFormFile? imageFile)
        {
            _logger.LogInformation("Starting to update category with id {Id}...", id);
            try
            {
                var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
                if (existingCategory == null) return NotFound();

                if (imageFile != null && imageFile.Length > 0)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories", existingCategory.picture);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories", fileName);

                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    updateDto.picture = fileName;
                }
                else
                {
                    updateDto.picture = existingCategory.picture;
                }
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateDto);
                if (updatedCategory == null) return NotFound();
                _logger.LogInformation("Updated category with id {Id} successfully.", id);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with id {Id}", id);
                return BadRequest(ex.Message);
            }
        }
        // Delete category
        [Authorize(Roles = "manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _logger.LogInformation("Starting to delete category with id {Id}...", id);
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null) return NotFound("category with the given ID was not found");
                var isDeleted = await _categoryService.DeleteCategoryAsync(id);
                if (isDeleted)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories", category.picture);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        _logger.LogInformation("Deleted physical file: " + category.picture);
                    }
                }
                _logger.LogInformation("Deleted category successfully.");
                if (!isDeleted) return NotFound("error ocuured while deleting");
                return Ok("deleted succesfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category with id {Id}.", id);
                return BadRequest("Internal server error occurred.");
            }
        }
    }
}