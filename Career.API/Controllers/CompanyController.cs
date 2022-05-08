using Career.Common.Logging;
using Career.Contract.Contracts.Company;
using Career.Service.Services.CompanyService;
using FluentValidationDemo.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Career.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompositeLogger _logger;
    private readonly ICompanyService _companyService;


    public CompanyController(
        ICompositeLogger logger,
        ICompanyService companyService
        )
    {
        _logger = logger;
        _companyService = companyService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(CompanyContract contract)
    {
        var companyConractValidator = new CompanyContractValidator();
        var validatorResult = companyConractValidator.Validate(contract);
        if (!validatorResult.IsValid)
            return BadRequest();

        await _companyService.Add(contract);

        return Ok();
    }
}