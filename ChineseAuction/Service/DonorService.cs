using AutoMapper;
using ChineseAuction.Dtos;
using ChineseAuction.Models;
using ChineseAuction.Repositoreis;

namespace ChineseAuction.Service
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRpository _donorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DonorService> _logger;
        public DonorService(IDonorRpository donorRepository, IMapper mapper, ILogger<DonorService> logger)
        {
            _donorRepository = donorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // Service methods for handling business logic related to Donor entity

        // get all donors
        public async Task<IEnumerable<ManagerGetDonorDto>> GetAllDonorsAsync()
        {
            var donors = await _donorRepository.GetAllDonorsAsync();
            return _mapper.Map<IEnumerable<ManagerGetDonorDto>>(donors);
        }

        // get donor by id
        public async Task<ManagerGetDonorDto?> GetDonorByIdAsync(int id)
        {
            var donor = await _donorRepository.GetDonorByIdAsync(id);
            if (donor == null)
            {
                _logger.LogWarning("Donor with id {DonorId} not found.", id);
                return null;
            }
            return _mapper.Map<ManagerGetDonorDto>(donor);
        }

        // add new donor
        public async Task<ManagerGetDonorDto> AddDonorAsync(CreateDonorDto donor)
        {
            if (await DonorEmailExistsAsync(donor.Email, -1))
            {
                _logger.LogWarning("Attempt to add donor with existing email: {Email}", donor.Email);
                throw new Exception("Email already exists");
            }
            donor.Password = HashPassword(donor.Password);
            var newDonor = _mapper.Map<Donor>(donor);
            await _donorRepository.AddDonorAsync(newDonor);
            return _mapper.Map<ManagerGetDonorDto>(newDonor);
        }

        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        // update donor
        public async Task<ManagerGetDonorDto?> UpdateDonorAsync(int id, CreateDonorDto donor)
        {
            var existingDonor = await _donorRepository.GetDonorByIdAsync(id);
            if (existingDonor == null)
            {
                _logger.LogWarning("Attempt to update non-existing donor with id: {DonorId}", id);
                return null;
            }
            if (donor.Email != existingDonor.Email && donor.Email != null)
            {
                if (await DonorEmailExistsAsync(donor.Email, id))
                {
                    _logger.LogWarning("Attempt to update donor with existing email: {Email}", donor.Email);
                    throw new Exception("User with the same email already exists.");
                }
            }
            if (donor.Password != null) existingDonor.Password = HashPassword(donor.Password);
            _mapper.Map(donor, existingDonor);
            existingDonor.Id = id;
            var updatedDonor = await _donorRepository.UpdateDonorAsync(existingDonor);
            if (updatedDonor == null)
            {
                _logger.LogError("Failed to update donor with id: {DonorId}", id);
                return null;
            }
            return _mapper.Map<ManagerGetDonorDto>(updatedDonor);
        }

        // delete donor
        public async Task<bool> DeleteDonorAsync(int id)
        {
            var existingDonor = await _donorRepository.GetDonorByIdAsync(id);
            if (existingDonor == null)
            {
                _logger.LogWarning("Attempt to delete non-existing donor with id: {DonorId}", id);
                return false;
            }
            await _donorRepository.DeleteDonorAsync(id);
            return true;
        }

        // Check if donor mail exists
        private async Task<bool> DonorEmailExistsAsync(string email, int v)
        {
            return await _donorRepository.DonorEmailExistsAsync(email, v);
        }

        // filter donor by name, email, gift name
        public async Task<IEnumerable<ManagerGetDonorDto>> GetFilteredDonorsAsync(string? name, string? email, string? giftName)
        {
            var donors = await _donorRepository.GetFilteredDonorsAsync(name, email, giftName);
            return _mapper.Map<IEnumerable<ManagerGetDonorDto>>(donors);
        }
    }
}
