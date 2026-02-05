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
        readonly IMediator mediador;
        protected Respuesta respuesta;
        private ILogger<MenuController> logger;

        public EnsayoController(ILogger<MenuController> logger, IMediator mediador)
        {
            this.mediador = mediador;
            this.respuesta = new Respuesta();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> InsertarEnsayo(InsertarEnsayoRequest ensayo)
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new InsertarEnsayoCom(ensayo)));
            return Ok(respuesta);
        }
    }
}
