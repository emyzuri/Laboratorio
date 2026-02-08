using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Usuarios;
using Core.Aplicacion.RespuestaUtilitario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class UsuarioController : BaseApiController
    {
        readonly IMediator mediador;
        protected Respuesta respuesta;
        private ILogger<MenuController> logger;

        public UsuarioController(ILogger<MenuController> logger, IMediator mediador)
        {
            this.mediador = mediador;
            this.respuesta = new Respuesta();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        public async Task<IActionResult> ValidarCliente([FromQuery] string usuario, [FromQuery] string password)
        {
            ValidarUsuarioCom validarUsuario = new(password, usuario);
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(validarUsuario));
            return Ok(respuesta);
        }
        [HttpGet]
        [Route("Usuarios")]

        public async Task<IActionResult> ConsultarClientes()
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new ConsultarUsuariosCom()));
            return Ok(respuesta);
        }
        [HttpGet("ListarUsuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () =>
                await mediador.Send(new ListarTodosUsuariosCom())
            );
            return Ok(respuesta);
        }
    }
}
