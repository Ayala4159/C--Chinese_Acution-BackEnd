using ChineseAuction.Dtos;

namespace ChineseAuction.Service
{
    public interface IPackageService
    {
        Task<GetPackageDto> AddPackageAsync(CreatePackageDto createPackageDto);
        Task<IEnumerable<GetPackageDto>> GetAllPackagesAsync();
        Task<GetPackageDto?> GetPackageByIdAsync(int id);
    }
}