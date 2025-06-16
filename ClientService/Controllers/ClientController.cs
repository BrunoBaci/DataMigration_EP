using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientRepository _importService;

    public ClientController(IClientRepository importService)
    {
        _importService = importService;
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportClients(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file was uploaded.");

        using var stream = file.OpenReadStream();
        var result = await _importService.ImportClientsAsync(stream);
        return Ok(result);
    }
}