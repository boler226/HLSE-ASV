using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.DbContext;
using CompanyService.Infrastructure.UnitOfWork.Interfaces;

namespace CompanyService.Infrastructure.UnitOfWork {
    public class UnitOfWork(
        CompanyDbContext context,
        ICompanyRepository companyRepository
        ) : IUnitOfWork {
        public ICompanyRepository Companies => companyRepository;
        public async Task<int> SaveChangesAsync() =>
            await context.SaveChangesAsync();
    }
}
