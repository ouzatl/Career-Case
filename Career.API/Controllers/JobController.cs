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

        var result = await _jobService.Add(contract);

        return Ok();
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search(string text)
    {
        if (string.IsNullOrEmpty(text))
            return BadRequest();

        var result = await _jobService.Search(text);

        return Ok(result);
    }

    [HttpPost("AddBannedWord")]
    public async Task<IActionResult> AddBannedWord(string word)
    {
        if (string.IsNullOrEmpty(word))
            return BadRequest();

        var result = await _jobService.AddBannedWords(word);

        return Ok();
    }
}