using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportRepository _reportService;

    public ReportController(IReportRepository reportService)
    {
        _reportService = reportService;
    }

    [HttpPost("log")]
    public async Task<IActionResult> LogReport([FromBody] ImportResult result)
    {
        await _reportService.WriteReportAsync(result);
        return Ok("Report written.");
    }
}