using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.DTOs.Request;
using DeliveryApp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DeliveryApp.Controllers
{
    [Route("/motos")]
    [ApiController]
    public class MotorcycleController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateMotorcycleDTO dto, [FromServices] IMotorcycleService _service)
        {
            var result = await _service.CreateMotorcycleAsync(dto);
            if (result.Success)
                return Created();

            if (result.IsServerError)
            {
                return StatusCode(500, result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices] IMotorcycleService _service)
        {
            var result = await _service.GetMotorcyclesAsync();
            if (result.Success)
                return Ok(result.Result);

            if (result.IsServerError)
            {
                return StatusCode(500, result);
            }

            return BadRequest(result);
        }

        [HttpPut("/motos/{id}/placa")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Placa modificada com sucesso")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Dados inválidos")]
        public async Task<IActionResult> PutLicensePlateAsync(
            string id,
            [FromBody] UpdateLicensePlateDTO dto,
            [FromServices] IMotorcycleService _service
        )
        {
            dto.Id = id;
            var result = await _service.UpdateLicensePlateAsync(dto);
            var response = new { Mensagem = result.Message };

            if (result.Success)
                return Ok(response);

            if (result.IsServerError)
            {
                return StatusCode(500, response);
            }

            return BadRequest(response);
        }

        [HttpGet("/motos/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromServices] IMotorcycleService _service)
        {
            var result = await _service.GetMotorcycleByIdAsync(id);

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

        [HttpDelete("/motos/{id}")]
        public async Task<IActionResult> DeleteMotorcycleAsync(string id, [FromServices] IMotorcycleService _service)
        {
            var result = await _service.DeleteMotorcycleAsync(id);
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
