using AutoMapper;
using ChineseAuction.Dtos;
using ChineseAuction.Repositoreis;
using ChineseAuction.Models;
namespace ChineseAuction.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryRepository repository, IMapper mapper,ILogger<CategoryService> logger)
        {
            this._categoryRepository = repository;
            this._mapper = mapper;
            this._logger = logger;
        }

        // Get all categories
        public async Task<IEnumerable<GetCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<GetCategoryDto>>(categories);
        }

        // Get category by id
        public async Task<GetCategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with id {CategoryId} not found.", id);
                return null;
            }
            return _mapper.Map<GetCategoryDto>(category);
        }

        // Add new category
        public async Task<GetCategoryDto> AddCategoryAsync(CategoryDto createCategoryDto)
        {
            if (CategoryNameExistsAsync(createCategoryDto.Name,-1).Result)
            {
                _logger.LogWarning("Category name {CategoryName} already exists.", createCategoryDto.Name);
                throw new Exception("Category name already exists.");
            }
            var category = _mapper.Map<Category>(createCategoryDto);
            await _categoryRepository.AddCategoryAsync(category);
            return _mapper.Map<GetCategoryDto>(category);
        }

        // Update category
        public async Task<GetCategoryDto?> UpdateCategoryAsync(int id, CategoryDto updateCategoryDto)
        {
            if (await CategoryNameExistsAsync(updateCategoryDto.Name,id))
            {
                _logger.LogWarning("Category name {CategoryName} already exists.", updateCategoryDto.Name);
                throw new Exception("Category name already exists.");
            }
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                _logger.LogWarning("Category with id {CategoryId} not found.", id);
                return null;
            }
            _mapper.Map(updateCategoryDto, existingCategory);
            var updatedCategory = await _categoryRepository.UpdateCategoryAsync(existingCategory);
            if (updatedCategory == null)
            {
                _logger.LogError("Failed to update category with id {CategoryId}.", id);
                return null;
            }
            return _mapper.Map<GetCategoryDto>(updatedCategory);
        }
        // Delete category
        public async Task<bool> DeleteCategoryAsync(int id)
        {
           var exsitingCategory=await _categoryRepository.GetCategoryByIdAsync(id);
            if (exsitingCategory == null)
            {
                _logger.LogWarning("Category with id {CategoryId} not found.", id);
                return false;
            }
            await _categoryRepository.DeleteCategoryAsync(id);
            return true;
        }

        // Check if category name exists
        public async Task<bool> CategoryNameExistsAsync(string name,int id)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Id.Equals(id));
        }
    }
}