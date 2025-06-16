using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TextCheckerService.Models;
using TextCheckerService.Services;

namespace TextCheckerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuzzyMatchController : ControllerBase
{
    private readonly FuzzyMatcher _matcher;

    public FuzzyMatchController(FuzzyMatcher matcher)
    {
        _matcher = matcher;
    }

    [HttpPost("load-candidates")]
    public IActionResult LoadCandidates([FromBody] List<string> candidates)
    {
        _matcher.LoadCandidates(candidates);
        return Ok("Candidate list loaded successfully.");
    }

    [HttpGet("match")]
    public ActionResult<MatchTextResult> Match([FromQuery] string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return BadRequest("Input name is required.");

        var result = _matcher.Match(input);
        return Ok(result);
    }
}
