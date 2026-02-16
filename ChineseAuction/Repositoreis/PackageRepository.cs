using Microsoft.EntityFrameworkCore;
using ChineseAuction.Data;
using ChineseAuction.Models;

namespace ChineseAuction.Repositoreis
{
    public class PackageRepository : IPackageRepository
    {
        private readonly ChinesActionDbContext _context;
        public PackageRepository(ChinesActionDbContext context)
        {
            _context = context;
        }

        // Methods for CRUD operations on Package entity

        // get all packages -everyOne
        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _context.Packages.ToListAsync();
        }

        // get package by id -everyOne
        public async Task<Package?> GetPackageByIdAsync(int id)
        {
            return await _context.Packages.FindAsync(id);
        }

        // add new package -manager
        public async Task AddPackageAsync(Package package)
        {
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
        }
    }
}
