using Core.Aplicacion.Funciones.Comandos.Ubicacion;
using Core.Aplicacion.RespuestaUtilitario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class UbicacionController : BaseApiController
    {
        private readonly IMediator _mediador;
        private readonly ILogger<UbicacionController> _logger;

        public UbicacionController(ILogger<UbicacionController> logger, IMediator mediador)
        {
            _mediador = mediador;
            _logger = logger;
        }

        [HttpGet("Provincias")]
        public async Task<IActionResult> GetProvincias()
        {
            var respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerProvinciasCom()));
            return Ok(respuesta);
        }

        [HttpGet("Cantones/{idProvincia}")]
        public async Task<IActionResult> GetCantones(int idProvincia)
        {
            var respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerCantonesCom(idProvincia)));
            return Ok(respuesta);
        }

        [HttpGet("Parroquias/{idCanton}")]
        public async Task<IActionResult> GetParroquias(int idCanton)
        {
            var respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerParroquiasCom(idCanton)));
            return Ok(respuesta);
        }
    }
}