using AutoMapper;
using ChineseAuction.Services;
using ChineseAuction.Dtos;
using ChineseAuction.Models;
using ChineseAuction.Repositoreis;
using Chinese_Auction.Services;

namespace ChineseAuction.Service
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<PurchaseService> _logger;
        private readonly IUserRepository _userRepository;
        public PurchaseService(IPurchaseRepository purchaseRepository, IMapper mapper, IEmailService emailService, ILogger<PurchaseService> logger, IUserRepository userRepository)
        {
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
            _userRepository = userRepository;
        }

        // get all purchases - manager

        public async Task<IEnumerable<GetPurchaseDto>> GetAllPurchasesAsync()
        {
            var purchases = await _purchaseRepository.GetAllPurchasesAsync();
            return _mapper.Map<IEnumerable<GetPurchaseDto>>(purchases);
        }

        // get purchase by id - manager
        public async Task<GetPurchaseDto?> GetPurchaseByIdAsync(int id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                _logger.LogWarning($"Purchase with ID {id} not found.");
                return null;
            }
            return _mapper.Map<GetPurchaseDto>(purchase);
        }

        // add new purchase - user

        public async Task<IEnumerable<GetPurchaseDto>> AddPurchaseAsync(List<CreatePurchaseDto> purchaseDtos)
        {
            var uniqueGroupId = Guid.NewGuid().ToString();

            var purchases = purchaseDtos.Select(dto =>
            {
                var purchase = _mapper.Map<Purchase>(dto);
                purchase.UniquePackageId = uniqueGroupId;
                purchase.Pruchase_date = DateTime.Now;
                purchase.IsWon = false;
                return purchase;
            }).ToList();
            var savedPurchases = await _purchaseRepository.AddPurchasesRangeAsync(purchases);
            return _mapper.Map<IEnumerable<GetPurchaseDto>>(savedPurchases);
        }

        // update purchase - manager
        public async Task<GetPurchaseDto?> UpdatePurchaseAsync(int id, UpdatePurchaseDto purchase)
        {
            var existingPurchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (existingPurchase == null)
            {
                _logger.LogWarning($"Purchase with ID {id} not found for update.");
                return null;
            }
            _mapper.Map(purchase, existingPurchase);
            existingPurchase.Id = id;
            var updatedPurches = await _purchaseRepository.UpdatePurchaseAsync(existingPurchase);
            if (updatedPurches == null)
            {
                _logger.LogError($"Failed to update Purchase with ID {id}.");
                return null;
            }
            return _mapper.Map<GetPurchaseDto>(updatedPurches);
        }

        // get purchases by user id - manager
        public async Task<IEnumerable<GetPurchaseDto>> GetPurchasesByUserIdAsync(int userId)
        {
            var userPurchases = await _purchaseRepository.GetPurchasesByUserIdAsync(userId);
            if (userPurchases == null)
            {
                _logger.LogWarning($"No purchases found for User ID {userId}.");
                return Enumerable.Empty<GetPurchaseDto>();
            }
            return _mapper.Map<IEnumerable<GetPurchaseDto>>(userPurchases);
        }

        // get purchases by gift id - manager
        public async Task<IEnumerable<GetPurchaseDto>> GetPurchasesByGiftIdAsync(int giftId)
        {
            var giftPurchases = await _purchaseRepository.GetPurchasesByGiftIdAsync(giftId);
            if (giftPurchases == null)
            {
                _logger.LogWarning($"No purchases found for Gift ID {giftId}.");
                return Enumerable.Empty<GetPurchaseDto>();
            }
            return _mapper.Map<IEnumerable<GetPurchaseDto>>(giftPurchases);
        }

        // lottory winner
        public async Task<GetPurchaseDto?> GetWinnersByGiftIdAsync(int giftId)
        {
            var winner = await _purchaseRepository.GetWinnerByGiftIdAsync(giftId);
            if (winner == null)
            {
                _logger.LogWarning($"No winner found for Gift ID {giftId}.");
                return null;
            }
            return _mapper.Map<GetPurchaseDto>(winner);
        }

        // lottory function
        public async Task<GetPurchaseDto?> Lottory(int giftId)
        {
            IEnumerable<Purchase> allPurchases = await _purchaseRepository.GetPurchasesByGiftIdAsync(giftId);
            if (allPurchases == null || !allPurchases.Any())
            {
                _logger.LogWarning($"No purchases found for Gift ID {giftId} to conduct lottery.");
                return null;
            }

            var random = new Random();
            var allPurchasesList = allPurchases.ToList();
            var winner = allPurchasesList[random.Next(allPurchasesList.Count)];
            var winnerDto = _mapper.Map<GetPurchaseDto>(winner);
            winner.IsWon = true;
            await _purchaseRepository.UpdatePurchaseAsync(winner);
            await SendNotificationEmail(winnerDto);

            return _mapper.Map<GetPurchaseDto>(winner);
        }

        // send email to the winner
        private async Task SendNotificationEmail(GetPurchaseDto winner)
        {
            var user = await _userRepository.GetUserByIdAsync(winner.Id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found. Cannot send notification email.", winner.Id);
                return;
            }
            var recipientEmail = user.Email;
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                string subject = "עדכון לגבי ההגרלה";
                string message = "ברכותינו! עליית בגורל כזוכה עבור המתנה המבוקשת.";
                await _emailService.SendEmailAsync(recipientEmail, subject, message);
            }
        }

        // get sorted purchases
        public async Task<IEnumerable<GetPurchaseDto>> GetSortedPurchasesAsync(string sortBy)
        {
            var purchases = await _purchaseRepository.GetAllPurchasesAsync();
            if (sortBy == "value")
                purchases = purchases
                    .Where(p => p.Gift != null)
                    .OrderByDescending(p => p.Gift!.Value);
            else if (sortBy == "popularity")
                purchases = purchases
                    .Where(p => p.Gift != null)
                    .OrderByDescending(p => p.Gift!.Purchases_quantity);

            return _mapper.Map<IEnumerable<GetPurchaseDto>>(purchases);
        }
    }
}
