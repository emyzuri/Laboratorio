using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Usuarios;
using Core.Aplicacion.RespuestaUtilitario;
using Core.Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistrarUsuarioCom comando)
        {
            if (comando == null)
            {
                return BadRequest("Los datos del usuario son nulos.");
            }
            var resultado = await mediador.Send(comando);

            if (resultado)
            {
                return Ok(new { esExitoso = true, mensaje = "Usuario registrado correctamente." });
            }

            return BadRequest(new { esExitoso = false, mensaje = "No se pudo registrar el usuario." });
        }
        [HttpPut("actualizar-roles")] 
        public async Task<IActionResult> ActualizarRoles([FromBody] ActualizarRolRequest request)
        {
            if (request == null || request.Roles == null || !request.Roles.Any())
                return BadRequest(new { mensaje = "Debe seleccionar al menos un rol." });
            var result = await mediador.Send(request);

            if (!result)
                return BadRequest(new { mensaje = "No se pudieron actualizar los roles." });

            return Ok(new { esExitoso = true });
        }
        [HttpGet("ListarRoles")]
        public async Task<IActionResult> ListarRoles()
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () =>
                await mediador.Send(new ConsultarRolesCom())
            );
            return Ok(respuesta);
        }

    }
}
