using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Menu;
using Core.Aplicacion.Funciones.Comandos.Usuarios;
using Core.Aplicacion.RespuestaUtilitario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class MenuController : BaseApiController
    {
        readonly IMediator mediador;
        protected Respuesta respuesta;

        public MenuController(IMediator mediador)
        {
            this.mediador = mediador;
            this.respuesta = new Respuesta();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMenu()
        {
            respuesta = await mediador.Send(new ValidarMenuCom());
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("Menus")]
        public async Task<IActionResult> ConsultarMenus()
        {
            var menus = await mediador.Send(new ConsultarMenuCom());
            return Ok(menus);
        }
    }
}