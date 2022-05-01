using Career.Common.Logging;
using Career.Contract.Contracts.Job;
using Career.Service.Services.JobService;
using Microsoft.AspNetCore.Mvc;

namespace Career.API.Controllers;

[ApiController]
[Route("[controller]")]
public class JobController : ControllerBase
{
    private readonly ICompositeLogger _logger;
    private readonly IJobService _jobService;


    public JobController(
        ICompositeLogger logger,
        IJobService jobService
        )
    {
        _logger = logger;
        _jobService = jobService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(JobContract contract)
    {
        //filter yaz
        if (contract == null ||
        string.IsNullOrEmpty(contract.Position) ||
        string.IsNullOrEmpty(contract.Description) ||
        contract.AvailableFrom == null ||
        contract.CompanyId <= default(int))
            return BadRequest();

        await _jobService.Add(contract);

        return Ok();
    }

    [HttpGet(Name = "Search")]
    public async Task<IActionResult> Search(string key)
    {
        //await _jobService.Search(key);
        return Ok();
    }
}