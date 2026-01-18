using Microsoft.EntityFrameworkCore;
using ChineseAuction.Data;
using ChineseAuction.Models;
using ChineseAuction.Dtos;
namespace ChineseAuction.Repositoreis
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ChinesActionDbContext _context;
        public CategoryRepository(ChinesActionDbContext context)
        {
            _context = context;
        }

        // get all categories -everyOne
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        // get category by id -everyOne
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        // add new category -manager
        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        // update category -manager
        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            var existing = await _context.Categories.FindAsync(category.Id);
            if (existing == null) { return null; }
            existing.Name = category.Name;
            _context.Categories.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        // delete category -manager
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
