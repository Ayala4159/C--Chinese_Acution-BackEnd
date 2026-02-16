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
        public async Task<IEnumerable<GetGiftDto>> GetAllGiftsAsync()
        {
            var gifts = await _giftRepository.GetAllGiftsAsync();
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
            if (existingGift.Purchase.Any())
            {
                throw new InvalidOperationException("לא ניתן לערוך מתנה שיש לה רכישות");
            }
            _mapper.Map(giftDto, existingGift);
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
            if (existingGift.Purchase.Any())
            {
                throw new InvalidOperationException("לא ניתן לערוך מתנה שיש לה רכישות");
            }
            await _giftRepository.DeleteGiftAsync(id);
            return true;
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
            var gifts = await _giftRepository.GetAllGiftsAsync();
            if (sortBy == "value")
                gifts =gifts.OrderByDescending(p => p.Value);
            else if (sortBy == "category")
                gifts = gifts
                    .Where(p => p.Category != null)
                    .OrderByDescending(p => p.Category!.Name);

            return _mapper.Map<IEnumerable<GetPurchaseDto>>(gifts);
        }
        //get gift by category
        public async Task<IEnumerable<GetGiftDto?>> GetGiftByCategoryAsync(int catId)
        {
            var gifts = await _giftRepository.GetGiftsByCategoryAsync(catId);
            return _mapper.Map<IEnumerable<GetGiftDto>>(gifts);
        }
    }
}