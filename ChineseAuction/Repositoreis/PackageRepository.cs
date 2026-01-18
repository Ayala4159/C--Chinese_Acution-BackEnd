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

        // update package -manager
        public async Task<Package?> UpdatePackageAsync(Package package)
        {
            var existing = await _context.Packages.FindAsync(package.Id);
            if (existing == null) { return null; }
            existing.Name = package.Name;
            existing.Description = package.Description;
            existing.Price = package.Price;
            await _context.SaveChangesAsync();
            return existing;
        }

        // delete package -manager
        public async Task DeletePackageAsync(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package != null)
            {
                _context.Packages.Remove(package);
                await _context.SaveChangesAsync();
            }
        }
    }
}
