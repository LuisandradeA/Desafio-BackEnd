using DeliveryApp.Application.DTOs.Request.Rentals;
using DeliveryApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApp.Controllers
{
    [Route("/locacao")]
    [ApiController]
    public class RentalsController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateRentalDTO dto, [FromServices] IRentalService _service)
        {
            var result = await _service.CreateRentalAsync(dto);
            var response = new { Mensagem = result.Message };

            if (result.Success)
                return Ok();

            if (result.IsServerError)
            {
                return StatusCode(500, response);
            }

            return BadRequest(response);
        }

        [HttpGet()]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromServices] IRentalService _service)
        {
            var result = await _service.GetRentalByIdAsync(id);

            var response = new { Mensagem = result.Message };

            if (result.Success)
                return Ok(result.Result);

            if (result.IsServerError)
            {
                return StatusCode(500, response);
            }

            if (result.IsBadRequest)
                return BadRequest(response);

            return NotFound(response);
        }
    }
}
