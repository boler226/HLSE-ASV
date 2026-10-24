using CompanyService.Domain.Entities;

namespace CompanyService.Domain.Interfaces {
    public interface ICompanyRepository {
        Task<List<Company>> GetAllAsync(CancellationToken cancellationToken);
        Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Company?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddAsync(Company user, CancellationToken cancellationToken);
        Task UpdateAsync(Company user, CancellationToken cancellationToken);
        Task DeleteAsync(Company user, CancellationToken cancellationToken);
    }
}
