using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.RespuestaUtilitario;
using Core.Dominio.Request.Clientes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Web.PalicacionAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de clientes. Permite realizar operaciones relacionadas con los clientes, como consultar, actualizar, eliminar, insertar y activar clientes en el sistema. Este controlador utiliza el patrón Mediator para manejar las solicitudes y respuestas, facilitando la separación de responsabilidades y mejorando la mantenibilidad del código. Además, se implementa un sistema de respuesta estandarizado a través de la clase Respuesta, que proporciona una estructura consistente para las respuestas de la API, incluyendo información sobre el éxito o fracaso de las operaciones y los datos relevantes. El controlador también utiliza un logger para registrar eventos importantes y facilitar la depuración y el monitoreo del sistema.
    /// </summary>
    public class ClienteController(IMediator mediador, ILogger<MenuController> logger) : BaseApiController
    {
        /// <summary>
        /// Inyeccion de dependencias del mediador, utilizado para manejar las solicitudes y respuestas de la API, facilitando la separación de responsabilidades y mejorando la mantenibilidad del código. El mediador se inyecta a través del constructor para permitir su uso en los métodos del controlador, donde se envían comandos y consultas para realizar las operaciones relacionadas con los clientes en el sistema.
        /// </summary>
        readonly IMediator mediador = mediador;

        /// <summary>
        /// Objeto respuesta utilizado para estandarizar las respuestas de la API, proporcionando una estructura consistente para las respuestas, incluyendo información sobre el éxito o fracaso de las operaciones y los datos relevantes. Este objeto se utiliza en los métodos del controlador para construir las respuestas que se envían al cliente, asegurando que todas las respuestas sigan un formato uniforme y faciliten la interpretación de los resultados por parte del cliente.
        /// </summary>
        protected Respuesta respuesta = new();

        /// <summary>
        /// Logger utilizado para registrar eventos importantes y facilitar la depuración y el monitoreo del sistema. Este logger se inyecta a través del constructor para permitir su uso en los métodos del controlador, donde se pueden registrar mensajes de información, advertencia o error relacionados con las operaciones realizadas en el sistema, ayudando a identificar problemas y mejorar la calidad del código.
        /// </summary>
        private readonly ILogger<MenuController> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// Validar en que parte se llama este metodo en el front
        //[HttpGet]
        //public async Task<IActionResult> ValidarCliente([FromHeader] int idCliente)
        //{
        //    ValidarClienteCom validarCliente = new(idCliente);
        //    respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(validarCliente));
        //    return Ok(respuesta);
        //}

        /// <summary>
        /// Consulta de clientes. Permite obtener una lista de clientes registrados en el sistema, facilitando la gestión de información y la toma de decisiones basada en los datos de los clientes.
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        [Route("Clientes")]
        public async Task<IActionResult> ConsultarClientes()
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new ConsultarClienteCom()));
            return Ok(respuesta);
        }

        /// <summary>
        /// Actualización de cliente. Permite modificar la información de un cliente existente en el sistema, facilitando la gestión de datos y asegurando que la información del cliente esté actualizada y sea precisa. Este método recibe un objeto ActualizarClienteCom con los datos del cliente a actualizar, incluyendo su ID para identificar el registro a modificar, y devuelve una respuesta indicando el resultado de la operación. La actualización se realiza a través de un procedimiento almacenado llamado "spu_clientes", que se encarga de manejar la lógica de actualización y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="cliente">Cliente a actualizar</param>
        /// <returns>Cliente</returns>
        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> ActualizarCliente([FromBody] ActualizarClienteRequest actualizarClienteRequest)
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new ActualizarClienteCom(actualizarClienteRequest)));
            return Ok(respuesta);
        }

        /// <summary>
        /// Elimina cliente. Permite desactivar un cliente existente en el sistema, facilitando la gestión de datos y asegurando que la información del cliente esté actualizada y sea precisa. Este método recibe el ID del cliente a eliminar a través de un encabezado HTTP, y devuelve una respuesta indicando el resultado de la operación. La eliminación se realiza a través de un procedimiento almacenado llamado "spu_clientes_estado", que se encarga de manejar la lógica de desactivación y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente</param>
        /// <returns>Desactiva cliente</returns>
        [HttpDelete]
        [Route("Eliminar")]
        public async Task<IActionResult> EliminarCliente([FromHeader] int idCliente)
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new EliminarClienteCom(idCliente)));
            return Ok(respuesta);
        }

        /// <summary>
        /// Inserta cliente. Permite agregar un nuevo cliente al sistema, facilitando la gestión de datos y asegurando que la información del cliente esté actualizada y sea precisa. Este método recibe un objeto CrearClienteRequest con los datos del cliente a insertar, y devuelve una respuesta indicando el resultado de la operación, incluyendo la información del cliente creado, como su ID generado por la base de datos. La inserción se realiza a través de un procedimiento almacenado llamado "spi_clientes", que se encarga de manejar la lógica de inserción y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="comando">Obejto cliente</param>
        /// <returns>Cliente</returns>
        [HttpPost]
        [Route("Insertar")]
        public async Task<IActionResult> InsertarCliente([FromBody] CrearClienteRequest comando)
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new CrearClienteCom(comando)));
            return Ok(respuesta);
        }

        [HttpPost]
        [Route("Activar")]
        public async Task<IActionResult> ActivarCliente([FromBody] ActivarClienteRequest comando)
        {
            respuesta = await RespestaServicio.CrearRespuestaExito(logger, async () => await mediador.Send(new ActivarClienteCom(comando)));
            return Ok(respuesta);
        }
    }
}