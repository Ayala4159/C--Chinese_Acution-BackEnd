using ChineseAuction.Models;

namespace ChineseAuction.Repositoreis
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<Purchase>> AddPurchasesRangeAsync(IEnumerable<Purchase> purchases);
        Task<IEnumerable<Purchase>> GetAllPurchasesAsync();
        Task<Purchase?> GetPurchaseByIdAsync(int id);
        Task<IEnumerable<Purchase>> GetPurchasesByGiftIdAsync(int giftId);
        Task<IEnumerable<Purchase>> GetPurchasesByUserIdAsync(int userId);
        Task<Purchase?> GetWinnerByGiftIdAsync(int giftId);
        Task<Purchase?> UpdatePurchaseAsync(Purchase purchase);
    }
}