using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.Funciones.Comandos.Usuarios;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    public class ClienteController : BaseApiController
    {
        readonly IMediator mediador;

        public ClienteController(IMediator mediador)
        {
            this.mediador = mediador;
        }

        [HttpGet]
        public async Task<IActionResult> ValidarCliente([FromHeader] int idCliente)
        {
            ValidarClienteCom validarCliente = new(idCliente);
            var cliente = await mediador.Send(validarCliente);
            return Ok(cliente);
        }

        [HttpGet]
        [Route("Clientes")]
        public async Task<IActionResult> ConsultarClientes()
        {
            return Ok(await mediador.Send(new ConsultarClienteCom()));
        }

        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarCliente([FromBody] ActualizarClienteCom cliente)
        {
            var resultado = await mediador.Send(cliente);
            return Ok(resultado);
        }

        [HttpDelete]
        [Route("Eliminar")]
        public async Task<IActionResult> EliminarCliente([FromHeader] int idCliente)
        {
            var resultado = await mediador.Send(new EliminarClienteCom(idCliente));

            if (resultado)
                return Ok(true);

            return BadRequest("No se pudo desactivar el cliente");
        }

        [HttpPost]
        [Route("Insertar")]
        public async Task<IActionResult> InsertarCliente([FromBody] CrearClienteCom comando)
        {
            var respuesta = await mediador.Send(comando);
            if (respuesta.Dato > 0)
            {
                return Ok(respuesta);
            }

            return BadRequest("No se pudo insertar el cliente");
        }
    }
}