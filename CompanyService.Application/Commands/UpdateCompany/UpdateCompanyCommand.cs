using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyService.Application.Behaviors;

namespace CompanyService.Application.Commands.UpdateCompany {
    public record UpdateCompanyCommand(Guid Id, string Name, string Email, string Address) : IRequest, ICachedQuery
    {
        public string CacheKey => $"company:{Id}";
    }
}
