namespace UserService.Domain.Interfaces
{
    public interface IEntityExistenceChecker
    {
        Task<bool> CompanyExistsAsync(Guid companyId, CancellationToken cancellationToken);
    }
}