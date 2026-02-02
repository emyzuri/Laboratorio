using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Menu;
using Core.Aplicacion.Funciones.Comandos.Usuarios;
using Core.Aplicacion.RespuestaUtilitario;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class MenuController : BaseApiController
    {
        readonly IMediator mediador;
        protected Respuesta respuesta;
        private ILogger<MenuController> logger;

        public MenuController(ILogger<MenuController> logger, IMediator mediador)
        {
            this.mediador = mediador;
            this.respuesta = new Respuesta();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMenu()
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () =>  await mediador.Send(new ValidarMenuCom()));
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("Menus")]
        public async Task<IActionResult> ConsultarMenus()
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new ValidarMenuCom()));

            return Ok(respuesta);
        }
    }
}