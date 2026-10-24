using Contracts.Requests;
using MassTransit;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Services
{
    public class EntityExistenceChecker(
        IRequestClient<CheckCompanyExistsRequest> client
        ) : IEntityExistenceChecker {
        public async Task<bool> CompanyExistsAsync(Guid companyId, CancellationToken cancellationToken) {
            var response = await client.GetResponse<CheckCompanyExistsResponse>(
                new CheckCompanyExistsRequest { CompanyId = companyId }, cancellationToken);
            return response.Message.Exists;
        }
    }
}
