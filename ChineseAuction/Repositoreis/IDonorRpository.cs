using ChineseAuction.Models;

namespace ChineseAuction.Repositoreis
{
    public interface IDonorRpository
    {
        Task AddDonorAsync(Donor donor);
        Task DeleteDonorAsync(int id);
        Task<bool> DonorEmailExistsAsync(string email, int id);
        Task<IEnumerable<Donor>> GetAllDonorsAsync();
        Task<Donor?> GetDonorByEmailAsync(string email);
        Task<Donor?> GetDonorByIdAsync(int id);
        Task<IEnumerable<Donor>> GetFilteredDonorsAsync(string? name, string? email, string? giftName);
        Task<Donor?> UpdateDonorAsync(Donor donor);
    }
}