using ChineseAuction.Data;
using ChineseAuction.Models;
using Microsoft.EntityFrameworkCore;

namespace ChineseAuction.Repositoreis
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ChinesActionDbContext _context;
        public PurchaseRepository(ChinesActionDbContext context)
        {
            _context = context;
        }
        // Methods for CRUD operations on Purchase entity

        // get all purchases - manager
        public async Task<IEnumerable<Purchase>> GetAllPurchasesAsync()
        {
            return await _context.Purchases.Include(p => p.Gift).ToListAsync();
        }

        // get purchase by id - manager
        public async Task<Purchase?> GetPurchaseByIdAsync(int id)
        {
            return await _context.Purchases.Include(p => p.Gift).FirstOrDefaultAsync(p => p.Id == id);
        }

        // add new purchase - user
        public async Task<IEnumerable<Purchase>> AddPurchasesRangeAsync(IEnumerable<Purchase> purchases)
        {
            await _context.Purchases.AddRangeAsync(purchases);
            await _context.SaveChangesAsync();
            return purchases;
        }

        // update purchase - manager
        public async Task<Purchase?> UpdatePurchaseAsync(Purchase purchase)
        {
            var existing = await _context.Purchases.FindAsync(purchase.Id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(purchase);
            await _context.SaveChangesAsync();
            return existing;
        }

        // get purchases by user id - user himself or manager
        public async Task<IEnumerable<Purchase>> GetPurchasesByUserIdAsync(int userId)
        {
            return await _context.Purchases.Include(p => p.Gift).Where(p => p.UserId == userId).ToListAsync();
        }

        // get purchases by gift id - manager
        public async Task<IEnumerable<Purchase>> GetPurchasesByGiftIdAsync(int giftId)
        {
            return await _context.Purchases.Where(p => p.GiftId == giftId).ToListAsync();
        }
        // get winner by gift id - manager
        public async Task<Purchase?> GetWinnerByGiftIdAsync(int giftId)
        {
            return await _context.Purchases
                .FirstOrDefaultAsync(p => p.GiftId == giftId && p.IsWon);
        }
    }
}
