using MediatR;

namespace CompanyService.Application.Commands.CreateCompany
{
    public record CreateCompanyCommand(string Name, string Email, string Address) : IRequest<Guid>;
}
