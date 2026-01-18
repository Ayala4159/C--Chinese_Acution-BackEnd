using ChineseAuction.Dtos;

namespace ChineseAuction.Service
{
    public interface IDonorService
    {
        Task<ManagerGetDonorDto> AddDonorAsync(CreateDonorDto donor);
        Task<bool> DeleteDonorAsync(int id);
        Task<IEnumerable<ManagerGetDonorDto>> GetAllDonorsAsync();
        Task<ManagerGetDonorDto?> GetDonorByIdAsync(int id);
        Task<IEnumerable<ManagerGetDonorDto>> GetFilteredDonorsAsync(string? name, string? email, string? giftName);
        Task<ManagerGetDonorDto?> UpdateDonorAsync(int id, CreateDonorDto donor);
    }
}