using FlightsSearch.DAO.APIService;
using FlightsSearch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightsSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightResultsController : ControllerBase
    {
        private readonly FlightService _flightService;

        public FlightResultsController(FlightService amadeusFlightService)
        {
            _flightService = amadeusFlightService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchFlights([FromBody] FlightSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.originLocationCode) ||
                string.IsNullOrWhiteSpace(request.destinationLocationCode) ||
                string.IsNullOrWhiteSpace(request.departureDate) ||
                request.adults <= 0)
            {
                return BadRequest("Origin, Destination, DepartureDate, and Adults are required fields.");
            }

            var flightOffers = await _flightService.GetFlightOffersAsync(
                request.originLocationCode,
                request.destinationLocationCode,
                request.departureDate,
                request.adults,
                request.currencyCode
            );

            return Ok(flightOffers);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetFlightOffers([FromQuery] string origin, [FromQuery] string destination, [FromQuery] string departureDate, [FromQuery] int? adults, [FromQuery] string? currency)
        {
            if (string.IsNullOrWhiteSpace(origin) ||
                string.IsNullOrWhiteSpace(destination) ||
                string.IsNullOrWhiteSpace(departureDate) ||
                adults <= 0)
            {
                return BadRequest("Origin, Destination, DepartureDate, and Adults are required fields.");
            }

            var flightOffers = await _flightService.GetFlightOffersAsync(
                origin,
                destination,
                departureDate,
                adults,
                currency
            );

            return Ok(flightOffers);
        }
    }
}
