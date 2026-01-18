using ChineseAuction.Models;

namespace ChineseAuction.Repositoreis
{
    public interface IPackageRepository
    {
        Task AddPackageAsync(Package package);
        Task DeletePackageAsync(int id);
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<Package?> GetPackageByIdAsync(int id);
        Task<Package?> UpdatePackageAsync(Package package);
    }
}