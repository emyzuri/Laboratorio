using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.RespuestaUtilitario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class ClienteController : BaseApiController
    {
        /// <summary>
        /// Instacia de servicio mediador
        /// </summary>
        readonly IMediator mediador;

        public ClienteController(IMediator mediador)
        {
            this.mediador = mediador;
        }
        //protected Respuesta respuesta;

        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            ValidarClienteCom usuario = new("12345", "usuarioPrueba");
            var cliente = await mediador.Send(usuario);
            return Ok("Servicio de Clientes");
        }
    }
}
