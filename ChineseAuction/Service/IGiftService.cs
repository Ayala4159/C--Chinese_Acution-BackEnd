using ChineseAuction.Dtos;

namespace ChineseAuction.Service
{
    public interface IGiftService
    {
        Task<GetGiftDto> AddGiftAsync(CreateGiftDto giftDto);
        Task<bool> DeleteGiftAsync(int id);
        Task<IEnumerable<GetGiftDto>> GetAllApprovedGiftsAsync();
        Task<IEnumerable<GetGiftDto>> GetAllUnapprovedGiftsAsync();
        Task<GetGiftDto?> GetGiftByIdAsync(int id);
        Task<GetGiftDto?> UpdateGiftAsync(int id, CreateGiftDto giftDto);
        Task<UserUpdateGiftDto?> UpdateGiftPurchasesQuantityAsync(int giftId);
        Task<bool> UpdateApprovalStatusAsync(ApproveGiftDto gift);
        Task<IEnumerable<GetGiftDto>> GetFilteredGiftsAsync(string? giftName, string? donorName, int? minPurchases);
        Task<IEnumerable<GetPurchaseDto>> GetSortedGiftAsync(string sortBy);
    }
}