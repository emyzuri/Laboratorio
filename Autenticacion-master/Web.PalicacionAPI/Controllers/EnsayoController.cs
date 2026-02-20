using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Ensayo;
using Core.Aplicacion.RespuestaUtilitario;
using Core.Dominio.Request.Ensayos;
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
        public async Task<IActionResult> InsertarEnsayo([FromBody] InsertarEnsayoRequest request)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new InsertarEnsayoCom(request)));

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
        [HttpGet("Catalogo")]
        public async Task<IActionResult> ObtenerCatalogoEnsayo()
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerCatalogoCom()));
            return Ok(_respuesta);
        }
        [HttpPost("InsertarAbono")]
        public async Task<IActionResult> InsertarAbono([FromBody] InsertarAbonoCom comando)
        {
            var usuarioHeader = Request.Headers["Usuario"].ToString();
            comando.Usuario = !string.IsNullOrEmpty(usuarioHeader) ? usuarioHeader : "SISTEMA";

            var resultado = await _mediador.Send(comando);
            return Ok(new { esExitoso = resultado });
        }
        [HttpGet("Detallados")]
        public async Task<IActionResult> ObtenerEnsayosDetallados([FromQuery] int idPrueba)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerEnsayoCom(idPrueba)));

            return Ok(_respuesta);
        }
    }
}