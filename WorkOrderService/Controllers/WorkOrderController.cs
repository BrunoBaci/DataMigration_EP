using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly IWorkOrderRepository _WorkOrderService;

    public WorkOrderController(IWorkOrderRepository importService)
    {
        _WorkOrderService = importService;
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportWorkOrders(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file was uploaded.");

        using var stream = file.OpenReadStream();
        var result = await _WorkOrderService.ImportWorkOrdersAsync(stream);
        return Ok(result);
    }
}