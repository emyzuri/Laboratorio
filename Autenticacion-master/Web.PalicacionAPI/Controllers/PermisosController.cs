using Core.Aplicacion.Funciones.Comandos.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly IMediator _mediador;
        public PermisosController(IMediator mediador) => _mediador = mediador;

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar() => Ok(await _mediador.Send(new ListarPermisosCom()));

        [HttpDelete("Quitar/{id}")]
        public async Task<IActionResult> Quitar(int id)
            => Ok(await _mediador.Send(new QuitarPermisoCom { IdUsuarioRol = id }));
    }
}
