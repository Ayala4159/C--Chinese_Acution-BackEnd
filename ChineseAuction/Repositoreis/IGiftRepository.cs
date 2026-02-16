using ChineseAuction.Models;

namespace ChineseAuction.Repositoreis
{
    public interface IGiftRepository
    {
        Task<Gift> AddGiftAsync(Gift gift);
        Task DeleteGiftAsync(int id);
        Task<IEnumerable<Gift>> GetAllGiftsAsync();
        Task<IEnumerable<Gift>> GetFilteredGiftsAsync(string? giftName, string? donorName, int? minPurchases);
        Task<Gift?> GetGiftByIdAsync(int id);
        Task<IEnumerable<Gift>> GetGiftsByCategoryAsync(int categoryId);
        Task<IEnumerable<Gift>> GetGiftsByDonorAsync(int donorId);
        Task<Gift?> UpdateGiftAsync(Gift gift);
        Task<Gift?> UpdateGiftPurchasesQuantityAsync(int giftId);
        Task<Gift?> UpdateGiftLotteryAsync(int giftId);

    }
}