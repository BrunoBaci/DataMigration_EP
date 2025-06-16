using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TechnicianController : ControllerBase
{
    private readonly ITechnicianRepository _importService;

    public TechnicianController(ITechnicianRepository importService)
    {
        _importService = importService;
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportTechnicians(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file was uploaded.");

        using var stream = file.OpenReadStream();
        var result = await _importService.ImportTechniciansAsync(stream);
        return Ok(result);
    }
}