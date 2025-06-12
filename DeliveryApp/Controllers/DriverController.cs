using DeliveryApp.Application.DTOs.Request.Driver;
using DeliveryApp.Application.DTOs.Response;
using DeliveryApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApp.Controllers
{
    [Route("/entregadores")]
    [ApiController]
    public class DriverController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateDriverDTO dto, [FromServices] IDriverService _service)
        {
            var result = await _service.CreateDriverAsync(dto);
            var response = new { Mensagem = result.Message };

            if (result.Success)
                return Ok();

            if (result.IsServerError)
            {
                return StatusCode(500, response);
            }

            return BadRequest(response);
        }

        [HttpPost]
        [Route("/{id}")]
        public async Task<IActionResult> UploadLicenseAsync(string id, [FromBody] UploadLicenseDTO dto, [FromServices] IDriverService _service)
        {
            var result = await _service.UpdateLicensePlateImageAsync(id, dto.Base64Image);
            var response = new { Mensagem = result.Message };

            if (result.Success)
                return Ok(response);

            if (result.IsServerError)
            {
                return StatusCode(500, response);
            }

            return BadRequest(response);
        }
    }
}
