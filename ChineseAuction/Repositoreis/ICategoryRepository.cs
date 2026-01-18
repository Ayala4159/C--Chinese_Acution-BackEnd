using ChineseAuction.Models;

namespace ChineseAuction.Repositoreis
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> UpdateCategoryAsync(Category category);
    }
}