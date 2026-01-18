using ChineseAuction.Dtos;
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
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto createCategoryDto)
        {
            _logger.LogInformation("Starting to add new category...");
            try
            {
                GetCategoryDto category = await _categoryService.AddCategoryAsync(createCategoryDto);
                _logger.LogInformation("Added new category successfully with id {Id}.", category.Id);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error occurred while adding new category ");
                return BadRequest(ex.Message);
            }
        }
        // Update category
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto updateCategoryDto)
        {
            _logger.LogInformation("Starting to update category with id {Id}...", id);
            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
                if (updatedCategory == null) return NotFound();
                _logger.LogInformation("Updated category with id {Id} successfully.", id);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error occurred while updating category with id {Id}", id);
                return BadRequest(ex.Message);
            }
        }
        // Delete category
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _logger.LogInformation("Starting to delete category with id {Id}...", id);
            try
            {
                var isDeleted = await _categoryService.DeleteCategoryAsync(id);
                if (!isDeleted) return NotFound("The id:" + id + " ,did not found🤚");
                _logger.LogInformation("Deleted category with id {Id} successfully.", id);
                return Ok("Category deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error occurred while deleting category with id {Id}.", id);
                return BadRequest("Internal server error occurred.");
            }
        }
    }
}
