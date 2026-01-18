using AutoMapper;
using ChineseAuction.Dtos;
using ChineseAuction.Models;
using ChineseAuction.Repositoreis;

namespace ChineseAuction.Service
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _giftRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GiftService> _logger;
        public GiftService(IGiftRepository giftRepository, IMapper mapper, ILogger<GiftService> logger)
        {
            _giftRepository = giftRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //get all approved gifts
        public async Task<IEnumerable<GetGiftDto>> GetAllApprovedGiftsAsync()
        {
            var gifts = await _giftRepository.GetAllApprovedGiftsAsync();
            return _mapper.Map<IEnumerable<GetGiftDto>>(gifts);
        }

        //get all none approved gifts
        public async Task<IEnumerable<GetGiftDto>> GetAllUnapprovedGiftsAsync()
        {
            var gifts = await _giftRepository.GetAllUnapprovedGiftsAsync();
            return _mapper.Map<IEnumerable<GetGiftDto>>(gifts);
        }

        //get gift by id
        public async Task<GetGiftDto?> GetGiftByIdAsync(int id)
        {
            var gift = await _giftRepository.GetGiftByIdAsync(id);
            if(gift == null) 
            {
                _logger.LogWarning("Gift with id {GiftId} not found.", id);
                return null;
            }
            return _mapper.Map<GetGiftDto>(gift);
        }

        //add gift
        public async Task<GetGiftDto> AddGiftAsync(CreateGiftDto giftDto)
        {
            var gift = _mapper.Map<Gift>(giftDto);
            var addedGift = await _giftRepository.AddGiftAsync(gift);
            return _mapper.Map<GetGiftDto>(addedGift);
        }

        //update gift
        public async Task<GetGiftDto?> UpdateGiftAsync(int id, CreateGiftDto giftDto)
        {
            var existingGift = await _giftRepository.GetGiftByIdAsync(id);
            if (existingGift == null)
            { 
                _logger.LogWarning("Gift with id {GiftId} not found for update.", id);
                return null;
            }
            var gift = _mapper.Map<Gift>(giftDto);
            existingGift.Id = id;
            var updatedGift = await _giftRepository.UpdateGiftAsync(existingGift);
            if (updatedGift == null)
            {
                _logger.LogError("Failed to update gift with id {GiftId}.", id);
                return null;
            }
            return  _mapper.Map<GetGiftDto>(updatedGift);
        }

        //update gift purchases quantity
        public async Task<UserUpdateGiftDto?> UpdateGiftPurchasesQuantityAsync(int giftId)
        {
            var existingGift = await _giftRepository.GetGiftByIdAsync(giftId);
            if (existingGift == null)
            {
                _logger.LogWarning("Gift with id {GiftId} not found for updating purchases quantity.", giftId);
                return null;
            }
            var updatedGift = await _giftRepository.UpdateGiftPurchasesQuantityAsync(giftId);
            if (updatedGift == null)
            {
                _logger.LogError("Failed to update purchases quantity for gift with id {GiftId}.", giftId);
                return null;
            }
            return _mapper.Map<UserUpdateGiftDto>(updatedGift);
        }

        //delete gift
        public async Task<bool> DeleteGiftAsync(int id)
        {
            var existingGift = await _giftRepository.GetGiftByIdAsync(id);
            if (existingGift == null)
            {
                _logger.LogWarning("Gift with id {GiftId} not found for deletion.", id);
                return false;
            }
            await _giftRepository.DeleteGiftAsync(id);
            return true;
        }

        //approve gift
        public async Task<bool> UpdateApprovalStatusAsync(ApproveGiftDto gift)
        {
            var existingGift = await _giftRepository.GetGiftByIdAsync(gift.Id);
            if (existingGift == null)
            {
                _logger.LogWarning("Gift with id {GiftId} not found for approval status update.", gift.Id);
                return false;
            }
            bool success = await _giftRepository.UpdateApprovalStatusAsync(gift.Id, gift.Is_approved);
            return success;
        }

        //filter gifts
        public async Task<IEnumerable<GetGiftDto>> GetFilteredGiftsAsync(string? giftName, string? donorName, int? minPurchases)
        {
            var gifts = await _giftRepository.GetFilteredGiftsAsync(giftName, donorName, minPurchases);
            return _mapper.Map<IEnumerable<GetGiftDto>>(gifts);
        }

        // get sorted purchases
        public async Task<IEnumerable<GetPurchaseDto>> GetSortedGiftAsync(string sortBy)
        {
            var gifts = await _giftRepository.GetAllApprovedGiftsAsync();
            if (sortBy == "value")
                gifts =gifts.OrderByDescending(p => p.Value);
            else if (sortBy == "category")
                gifts = gifts
                    .Where(p => p.Category != null)
                    .OrderByDescending(p => p.Category!.Name);

            return _mapper.Map<IEnumerable<GetPurchaseDto>>(gifts);
        }

    }
}