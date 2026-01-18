using ChineseAuction.Dtos;

namespace ChineseAuction.Service
{
    public interface IPurchaseService
    {
        Task<IEnumerable<GetPurchaseDto>> AddPurchaseAsync(List<CreatePurchaseDto> purchaseDtos);
        Task<IEnumerable<GetPurchaseDto>> GetAllPurchasesAsync();
        Task<GetPurchaseDto?> GetPurchaseByIdAsync(int id);
        Task<IEnumerable<GetPurchaseDto>> GetPurchasesByGiftIdAsync(int giftId);
        Task<IEnumerable<GetPurchaseDto>> GetPurchasesByUserIdAsync(int userId);
        Task<GetPurchaseDto?> GetWinnersByGiftIdAsync(int giftId);
        Task<GetPurchaseDto?> Lottory(int giftId);
        Task<GetPurchaseDto?> UpdatePurchaseAsync(int id, UpdatePurchaseDto purchase);
        Task<IEnumerable<GetPurchaseDto>> GetSortedPurchasesAsync(string sortBy);
    }
}