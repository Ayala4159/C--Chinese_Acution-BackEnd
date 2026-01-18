using ChineseAuction.Dtos;

namespace ChineseAuction.Service
{
    public interface ICategoryService
    {
        Task<GetCategoryDto> AddCategoryAsync(CategoryDto createCategoryDto);
        Task<bool> CategoryNameExistsAsync(string name, int id);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<GetCategoryDto>> GetAllCategoriesAsync();
        Task<GetCategoryDto?> GetCategoryByIdAsync(int id);
        Task<GetCategoryDto?> UpdateCategoryAsync(int id, CategoryDto updateCategoryDto);
    }
}