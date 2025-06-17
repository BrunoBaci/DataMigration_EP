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
        try
        {

            if (result == null)
            {
                return BadRequest("ImportResult is null");
            }

            await _reportService.WriteReportAsync(result);
            return Ok("Report written.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in LogReport: {ex.Message}");
            return StatusCode(500, $"Exception in LogReport: {ex.Message}");
        }
    }
}