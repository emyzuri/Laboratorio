using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Usuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class UsuarioController : BaseApiController
    {
        /// <summary>
        /// Instacia de servicio mediador
        /// </summary>
        readonly IMediator mediador;

        public UsuarioController(IMediator mediador)
        {
            this.mediador = mediador;
        }
        //protected Respuesta respuesta;

        [HttpGet]
        public async Task<IActionResult> ValidarCliente([FromHeader] string usuario, [FromHeader] string password)
        {
            ValidarUsuarioCom validarUsuario = new(password, usuario);
            var cliente = await mediador.Send(validarUsuario);
            return Ok(cliente);
        }
        [HttpGet]
        [Route("Usuarios")]

        public async Task<IActionResult> ConsultarClientes()
        {
            var cliente = await mediador.Send(new ConsultarClientesCom());
            return Ok(cliente);
        }
    }
}
