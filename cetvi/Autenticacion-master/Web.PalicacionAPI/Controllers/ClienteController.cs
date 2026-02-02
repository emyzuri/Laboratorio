using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Usuarios;
using Core.Aplicacion.RespuestaUtilitario;
using Core.DataAccess.Clientes.Interfaz;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class ClienteController : BaseApiController
    {
        readonly IMediator mediador;
        protected Respuesta respuesta;
        private ILogger<MenuController> logger;

        public ClienteController(IMediator mediador, ILogger<MenuController> logger)
        {
            this.mediador = mediador;
            this.respuesta = new Respuesta();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> ValidarCliente([FromHeader] int idCliente)
        {
            ValidarClienteCom validarCliente = new(idCliente);
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(validarCliente));
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("Clientes")]
        public async Task<IActionResult> ConsultarClientes()
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new ConsultarClienteCom()));
            return Ok(respuesta);
        }

        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarCliente([FromBody] ActualizarClienteCom cliente)
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(cliente));
            return Ok(respuesta);
        }

        [HttpDelete]
        [Route("Eliminar")]
        public async Task<IActionResult> EliminarCliente([FromHeader] int idCliente)
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new EliminarClienteCom(idCliente)));
            return Ok(respuesta);
        }

        [HttpPost]
        [Route("Insertar")]
        public async Task<IActionResult> InsertarCliente([FromBody] CrearClienteCom comando)
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => mediador.Send(comando));
            return Ok(respuesta);
        }
    }
}