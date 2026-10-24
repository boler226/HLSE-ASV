using CompanyService.Application.Commands.CreateCompany;
using CompanyService.Application.Commands.DeleteCompany;
using CompanyService.Application.Commands.UpdateCompany;
using CompanyService.Application.Queries.GetAllCompanies;
using CompanyService.Application.Queries.GetCompanyByEmail;
using CompanyService.Application.Queries.GetCompanyById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompanyService.API.Controllers {
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CompaniesController(IMediator mediator) : ControllerBase {
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var result = await mediator.Send(new GetAllCompaniesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            var result = await mediator.Send(new GetCompanyByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email) {
            var result = await mediator.Send(new GetCompanyByEmailQuery(email));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyCommand command) {
            var companyId = await mediator.Send(command);
            return Ok(companyId);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCompanyCommand command) {
            await mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Send(new DeleteCompanyCommand(id));
            return Ok(id);
        }
    }
}
