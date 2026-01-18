using AutoMapper;
using ChineseAuction.Dtos;
using ChineseAuction.Models;
using ChineseAuction.Repositoreis;

namespace ChineseAuction.Service
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PackageService> _logger;
        public PackageService(IPackageRepository repository, IMapper mapper, ILogger<PackageService> logger)
        {
            this._packageRepository = repository;
            this._mapper = mapper;
            _logger = logger;
        }
        // Methods for CRUD operations on Package entity

        // get all packages
        public async Task<IEnumerable<GetPackageDto>> GetAllPackagesAsync()
        {
            var packages = await _packageRepository.GetAllPackagesAsync();
            return _mapper.Map<IEnumerable<GetPackageDto>>(packages);
        }

        // get package by id
        public async Task<GetPackageDto?> GetPackageByIdAsync(int id)
        {
            var package = await _packageRepository.GetPackageByIdAsync(id);
            if (package == null)
            {
                _logger.LogWarning("Package with id {PackageId} not found.", id);
                return null;
            }
            return _mapper.Map<GetPackageDto>(package);
        }

        // add new package
        public async Task<GetPackageDto> AddPackageAsync(CreatePackageDto createPackageDto)
        {
            var package = _mapper.Map<Package>(createPackageDto);
            await _packageRepository.AddPackageAsync(package);
            return _mapper.Map<GetPackageDto>(package);
        }

        // update package
        public async Task<GetPackageDto?> UpdatePackageAsync(int id, CreatePackageDto updatePackageDto)
        {
            var existingPackage = await _packageRepository.GetPackageByIdAsync(id);
            if (existingPackage == null)
            {
                _logger.LogWarning("Package with id {PackageId} not found for update.", id);
                return null;
            }
            _mapper.Map(updatePackageDto, existingPackage);
            existingPackage.Id = id;
            var updatedPackage = await _packageRepository.UpdatePackageAsync(existingPackage);
            if (updatedPackage == null)
            {
                _logger.LogError("Failed to update package with id {PackageId}.", id);
                return null;
            }
            return _mapper.Map<GetPackageDto>(updatedPackage);
        }

        // delete package
        public async Task<bool> DeletePackageAsync(int id)
        {
            var existingPackage = await _packageRepository.GetPackageByIdAsync(id);
            if (existingPackage == null)
            {
                _logger.LogWarning("Package with id {PackageId} not found for deletion.", id);
                return false;
            }
            await _packageRepository.DeletePackageAsync(id);
            return true;
        }
    }
}
