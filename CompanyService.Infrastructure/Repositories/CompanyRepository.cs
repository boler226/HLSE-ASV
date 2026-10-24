using CompanyService.Domain.Entities;
using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CompanyService.Infrastructure.Repositories {
    public class CompanyRepository(
        CompanyDbContext context
        ) : ICompanyRepository {
        public async Task<List<Company>> GetAllAsync (CancellationToken cancellationToken) =>
            await context.Companies.ToListAsync(cancellationToken);

        public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => 
            await context.Companies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Company?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
            await context.Companies.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);

        public async Task AddAsync(Company company, CancellationToken cancellationToken) {
            await context.Companies.AddAsync(company, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Company company, CancellationToken cancellationToken) {
            context.Companies.Update(company);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Company company, CancellationToken cancellationToken) {
            context.Companies.Remove(company);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
