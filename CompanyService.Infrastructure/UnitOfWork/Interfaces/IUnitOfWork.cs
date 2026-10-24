using CompanyService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Infrastructure.UnitOfWork.Interfaces {
    public interface IUnitOfWork {
        ICompanyRepository Companies {  get; }
        Task<int> SaveChangesAsync();
    }
}
