using ChineseAuction.Data;
using ChineseAuction.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ChineseAuction.Repositoreis
{
    public class GiftRepository : IGiftRepository
    {
        private readonly ChinesActionDbContext _context;
        public GiftRepository(ChinesActionDbContext context)
        {
            _context = context;
        }
        // Methods for CRUD operations on Gift entity

        // get all approved gifts - everyOne
        public async Task<IEnumerable<Gift>> GetAllApprovedGiftsAsync()
        {
            return await _context.Gifts.Include(g => g.Category)
                .Where(g => g.Is_approved == true)
                .Include(g => g.Donor)
                .ToListAsync();
        }

        // get all not approved gifts - manager
        public async Task<IEnumerable<Gift>> GetAllUnapprovedGiftsAsync()
        {
            return await _context.Gifts.Include(g => g.Category)
                .Where(g => g.Is_approved == false)
                .Include(g => g.Donor)
                .ToListAsync();
        }
        // get gift by id - everyOne
        public async Task<Gift?> GetGiftByIdAsync(int id)
        {
            return await _context.Gifts.Include(g => g.Category).Include(g => g.Donor)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        // add new gift - manager
        public async Task<Gift> AddGiftAsync(Gift gift)
        {
            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
            return gift;
        }

        // update gift - manager
        public async Task<Gift?> UpdateGiftAsync(Gift gift)
        {
            var existing = await _context.Gifts.FindAsync(gift.Id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(gift);
            await _context.SaveChangesAsync();
            return existing;
        }

        // update gift purchases quantity - user
        public async Task<Gift?> UpdateGiftPurchasesQuantityAsync(int giftId)
        {
            int rowsAffected = await _context.Gifts
                .Where(g => g.Id == giftId)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    g => g.Purchases_quantity,
                    g => g.Purchases_quantity + 1));
            if (rowsAffected == 0) return null;
            return await _context.Gifts.FindAsync(giftId);
        }

        // delete gift - manager
        public async Task DeleteGiftAsync(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift != null)
            {
                _context.Gifts.Remove(gift);
                await _context.SaveChangesAsync();
            }
        }
        // get gifts by category - everyOne
        public async Task<IEnumerable<Gift>> GetGiftsByCategoryAsync(int categoryId)
        {
            return await _context.Gifts
                .Where(g => g.CategoryId == categoryId)
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .ToListAsync();
        }
        // get gifts by donor - donor or manager
        public async Task<IEnumerable<Gift>> GetGiftsByDonorAsync(int donorId)
        {
            return await _context.Gifts
                .Where(g => g.DonorId == donorId)
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .ToListAsync();
        }

        // approve gift - manager
        public async Task<bool> UpdateApprovalStatusAsync(int giftId, bool Is_approved)
        {
            int rowsAffected = await _context.Gifts
                .Where(g => g.Id == giftId)
                .ExecuteUpdateAsync(s => s.SetProperty(g => g.Is_approved, true));
            return rowsAffected > 0;
        }

        // filter gifts by name, description, donor name - everyOne
        public async Task<IEnumerable<Gift>> GetFilteredGiftsAsync(string? giftName, string? donorName, int? minPurchases)
        {
            var query = _context.Gifts.Include(g => g.Donor).AsQueryable();

            if (!string.IsNullOrEmpty(giftName))
                query = query.Where(g => g.Name.Contains(giftName));

            if (!string.IsNullOrEmpty(donorName))
                query = query.Where(g => (g.Donor != null && (g.Donor.First_name.Contains(donorName) || g.Donor.Last_name.Contains(donorName))));

            if (minPurchases.HasValue)
                query = query.Where(g => g.Purchases_quantity >= minPurchases.Value);

            return await query.ToListAsync();
        }
    }
}
