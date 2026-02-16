using ChineseAuction.Models;

namespace ChineseAuction.Repositoreis
{
    public interface IPackageRepository
    {
        Task AddPackageAsync(Package package);
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<Package?> GetPackageByIdAsync(int id);
    }
}