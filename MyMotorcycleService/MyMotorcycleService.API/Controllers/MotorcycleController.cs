using Microsoft.AspNetCore.Mvc;
using MyMotorcycleService.Application.Dtos.Requests;
using MyMotorcycleService.Application.Dtos.Responses;
using MyMotorcycleService.Application.Services.Interfaces;

namespace MyMotorcycleService.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MotorcyclesController : ControllerBase
  {
    private readonly IMotorcycleService _motorcycleService;

    public MotorcyclesController(IMotorcycleService motorcycleService)
    {
      _motorcycleService = motorcycleService;
    }

    [HttpGet("{licensePlate}")]
    public async Task<ActionResult<MotorcycleResponseDto>> GetMotorcycleByLicensePlate(string licensePlate)
    {
      var motorcycle = await _motorcycleService.GetMotorcycleByLicensePlate(licensePlate);
      if (motorcycle == null)
      {
        return NotFound();
      }
      return Ok(motorcycle);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MotorcycleResponseDto>>> GetAllMotorcycles()
    {
      var motorcycles = await _motorcycleService.GetAllMotorcycles();
      return Ok(motorcycles);
    }

    [HttpPost]
    public async Task<ActionResult> AddMotorcycle([FromBody] AddMotorcycleRequestDto motorcycle)
    {
      await _motorcycleService.AddMotorcycle(motorcycle);
      return CreatedAtAction(nameof(GetMotorcycleByLicensePlate), new { licensePlate = motorcycle.LicensePlate }, motorcycle);
    }

    [HttpPut("{licencePlate}")]
    public async Task<ActionResult> UpdateMotorcycle(string licensePlate, [FromBody] UpdateMotorcycleRequestDto motorcycle)
    {
      if (licensePlate != motorcycle.LicensePlate)
      {
        return BadRequest("License Plate in URL does not match License Plate in body");
      }

      await _motorcycleService.UpdateMotorcycle(motorcycle);
      return NoContent();
    }

    [HttpDelete("{licensePlate}")]
    public async Task<ActionResult> RemoveMotorcycle(string licensePlate)
    {
      await _motorcycleService.RemoveMotorcycleByLicensePlate(licensePlate);

      return NoContent();
    }

    [HttpPatch("motorcycle/{licensePlate}")]
    public async Task<ActionResult> PatchMotorcycle(string licensePlate, [FromBody] PatchLicensePlateDto updateDto)
    {
      var response = await _motorcycleService.UpdateLicensePlate(licensePlate, updateDto);

      if (response == null)
        return NotFound();

      else return Ok(response);
    }
  }
}
