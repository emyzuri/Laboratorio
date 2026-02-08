using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Ensayo;
using Core.Aplicacion.RespuestaUtilitario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class EnsayoController : BaseApiController
    {
        private readonly IMediator _mediador;
        protected Respuesta _respuesta;
        private readonly ILogger<EnsayoController> _logger;

        public EnsayoController(ILogger<EnsayoController> logger, IMediator mediador)
        {
            _mediador = mediador;
            _respuesta = new Respuesta();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> InsertarEnsayo(
            [FromBody] InsertarEnsayoCom request,
            [FromHeader] string IdSesion)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(request));

            return Ok(_respuesta);
        }

        [HttpGet("{idCliente}")]
        public async Task<IActionResult> ConsultarAbonos(int idCliente)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ConsultarAbonoCom(idCliente)));

            return Ok(_respuesta);
        }
        [HttpGet("Deudores")]
        public async Task<IActionResult> ObtenerClientesDeudores()
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerClientesDeudoresCom()));
            return Ok(_respuesta);
        }
    }
}