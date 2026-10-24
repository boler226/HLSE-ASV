using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Application.Commands.UpdateCompany {
    public record UpdateCompanyCommand(Guid Id, string Name, string Email, string Address) : IRequest;
}
