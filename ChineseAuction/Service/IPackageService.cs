using ChineseAuction.Dtos;

namespace ChineseAuction.Service
{
    public interface IPackageService
    {
        Task<GetPackageDto> AddPackageAsync(CreatePackageDto createPackageDto);
        Task<bool> DeletePackageAsync(int id);
        Task<IEnumerable<GetPackageDto>> GetAllPackagesAsync();
        Task<GetPackageDto?> GetPackageByIdAsync(int id);
        Task<GetPackageDto?> UpdatePackageAsync(int id, CreatePackageDto updatePackageDto);
    }
}