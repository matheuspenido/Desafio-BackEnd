using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyRentMotorService.API.Dtos;
using MyRentMotorService.Application.Dto;
using MyRentMotorService.Application.Dtos.Requests;
using MyRentMotorService.Application.Services;
using MyRentMotorService.Application.Services.Interfaces;

namespace MyRentMotorService.API.Controllers
{
    [ApiController]
  [Route("api/[controller]")]
  public class RentalController : ControllerBase
  {
    private readonly IRentalService _rentalService;
    private readonly IMapper _mapper;

    public RentalController(IRentalService rentalService, IMapper mapper)
    {
      _rentalService = rentalService;
      _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> CreateRental([FromBody] RequestCreateRentalDto dto)
    {
      var applicationDto = _mapper.Map<CreateRentalApplicationDto>(dto);
      var rental = await _rentalService.CreateRentalAsync(applicationDto);
      return Ok(rental);
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult> CompleteRental(Guid id, [FromBody] RequestCompleteRentalDto dto)
    {
      var applicationRequest = new RequestCompleteRentalApplicationDto
      {
        Id = id,
        ReturnDate = dto.ReturnDate,
      };


      var rental = await _rentalService.CompleteRentalAsync(applicationRequest);
      return Ok(rental);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetRentalById(Guid id)
    {
      var rental = await _rentalService.GetRentalByIdAsync(id);
      
      if (rental is not null) 
        return Ok(rental);

      return NotFound();
    }

    [HttpGet]
    public async Task<ActionResult> GetAllRentals()
    {
      var rentals = await _rentalService.GetAllRentalsAsync();
      return Ok(rentals);
    }

    [HttpPost("driverlicense/{driverLicense}/preview")]
    public async Task<ActionResult> GetRentalInfoById(string driverLicense, [FromBody] RequestPreviewInfoDto dto)
    {
      var applicationRequest = new RequestPreviewInfoApplicationDto
      {
        ReturnDate = dto.ReturnDate,
        DriverLicense = driverLicense,
      };

      var rental = await _rentalService.GetRentalPaymentPreviewAsync(applicationRequest);

      if (rental is not null)
        return Ok(rental);

      return NotFound();
    }
  }
}
