using Core.Aplicacion.Funciones.Comandos.Cliente;
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
    /// <summary>
    /// Controlador para la gestión de ensayos. Este controlador maneja las operaciones relacionadas con los ensayos, incluyendo la inserción de nuevos ensayos, la consulta de abonos asociados a un cliente, la obtención de clientes deudores, la consulta de catálogos relacionados con los ensayos, y la generación de reportes detallados de ensayos realizados en rangos de fechas específicos. Además, permite filtrar los ensayos por cédula del cliente y rango de fechas, facilitando el análisis y seguimiento de las actividades de ensayo.
    /// </summary>
    public class EnsayoController(ILogger<EnsayoController> logger, IMediator mediador) : BaseApiController
    {
        /// <summary>
        /// Iyeccion de dependencias del mediador, utilizado para manejar las solicitudes y respuestas de la API, facilitando la separación de responsabilidades y mejorando la mantenibilidad del código. El mediador se inyecta a través del constructor para permitir su uso en los métodos del controlador, donde se envían comandos y consultas para realizar las operaciones relacionadas con los ensayos en el sistema.
        /// </summary>
        private readonly IMediator _mediador = mediador;

        /// <summary>
        /// Objeto respuesta utilizado para estandarizar las respuestas de la API, proporcionando una estructura consistente para las respuestas, incluyendo información sobre el éxito o fracaso de las operaciones y los datos relevantes. Este objeto se utiliza en los métodos del controlador para construir las respuestas que se envían al cliente, asegurando que todas las respuestas sigan un formato uniforme y faciliten la interpretación de los resultados por parte del cliente.
        /// </summary>
        protected Respuesta _respuesta = new();

        /// <summary>
        /// Logger utilizado para registrar eventos importantes y facilitar la depuración y el monitoreo del sistema. Este logger se inyecta a través del constructor para permitir su uso en los métodos del controlador, donde se pueden registrar mensajes de información, advertencia o error relacionados con las operaciones realizadas en el sistema, ayudando a identificar problemas y mejorar la calidad del código.
        /// </summary>
        private readonly ILogger<EnsayoController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpPost]
        public async Task<IActionResult> InsertarEnsayo([FromBody] InsertarEnsayoRequest request)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new InsertarEnsayoCom(request)));

            return Ok(_respuesta);
        }

        [HttpGet("{idCliente}")]
        public async Task<IActionResult> ConsultarAbonos(int idCliente)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ConsultarAbonoCom(idCliente)));

            return Ok(_respuesta);
        }

        /// <summary>
        /// Consulta de clientes deudores. Permite obtener una lista de clientes que tienen pagos pendientes, facilitando la gestión de cobros y seguimiento de cuentas por cobrar.
        /// </summary>
        /// <returns>Lista de clientes deudores</returns>
        [HttpGet("Deudores")]
        public async Task<IActionResult> ObtenerClientesDeudores()
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerClientesDeudoresCom()));

            return Ok(_respuesta);
        }

        [HttpGet("Catalogo/{idPadre}")]
        public async Task<IActionResult> GetCatalogo(int idPadre)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerCatalogoCom(idPadre)));

            return Ok(_respuesta);
        }

        [HttpPost("InsertarAbono")]
        public async Task<IActionResult> InsertarAbono([FromBody] InsertarAbonoCom comando)
        {
            var usuarioHeader = Request.Headers["Usuario"].ToString();
            comando.Usuario = !string.IsNullOrEmpty(usuarioHeader) ? usuarioHeader : "SISTEMA";

            var resultado = await _mediador.Send(comando);
            return Ok(new { esExitoso = resultado });
        }

        [HttpGet("Detallados")]
        public async Task<IActionResult> ObtenerEnsayosDetallados([FromQuery] int idPrueba)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerEnsayoCom(idPrueba)));

            return Ok(_respuesta);
        }

        /// <summary>
        /// Consulta de ensayos por rango de fechas. Permite filtrar los ensayos realizados dentro de un período determinado, facilitando la generación de reportes y análisis de datos históricos.
        /// </summary>
        /// <param name="fechaInicio">Fecha inicio de la consula</param>
        /// <param name="fechaFin">Fecha fin de la consulta</param>
        /// <returns>Lista de ensayos</returns>
        [HttpGet("EnsayoFecha")]
        public async Task<IActionResult> ObtenerEnsayoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ObtenerEnsayoFechaCom(fechaInicio, fechaFin)));

            return Ok(_respuesta);
        }

        /// <summary>
        /// Genera pdf con el reporte de ensayos realizados en un rango de fechas específico. Permite a los usuarios obtener un informe detallado de los ensayos realizados dentro del período seleccionado, facilitando el análisis y seguimiento de las actividades de ensayo.
        /// </summary>
        /// <returns>Reporte en formato .pdf</returns>
        [HttpGet("ReportePorFecha")]
        public async Task<IActionResult> ReportePorFecha()
        {
            byte[] pdfBytes = await _mediador.Send(
                new ReporteEnsayoPorFechaCom());

            return File(
                pdfBytes,
                "application/pdf",
                $"ReporteEnsayos_{DateTime.Now:g}.pdf"
            );
        }

        /// <summary>
        /// Consulta de ensayos por cédula y rango de fechas. Permite filtrar los ensayos realizados por un cliente específico dentro de un período determinado.
        /// </summary>
        /// <param name="cedula">Cedula del cliente</param>
        /// <param name="fechaInicio">Fecha inicio de la consulta</param>
        /// <param name="fechaFin">Fecha Fin de la consulta</param>
        /// <returns>Lista de ensayos</returns>
        [HttpGet("ConsultarPorCedula")]
        public async Task<IActionResult> ConsultarPorCedula([FromQuery] string cedula, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            _respuesta = await RespestaServicio.CrearRespuestaExito(_logger, async () =>
                await _mediador.Send(new ConsultarEnsayosClienteCom(cedula, fechaInicio, fechaFin)));

            return Ok(_respuesta);
        }

        /// <summary>
        /// Genera pdf con el reporte de ensayos realizados en un rango de fechas específico. Permite a los usuarios obtener un informe detallado de los ensayos realizados dentro del período seleccionado, facilitando el análisis y seguimiento de las actividades de ensayo.
        /// </summary>
        /// <param name="fechaInicio">Fecha inicio de filtro</param>
        /// <param name="fechaFin">Fecha fin del filtro</param>
        /// <returns>Reporte en formato .pdf</returns>
        [HttpGet("ReportePorCliente")]
        public async Task<IActionResult> ReportePorCliente()
        {
            byte[] pdfBytes = await _mediador.Send(
                new ReporteEnsayoClienteCom());

            return File(
                pdfBytes,
                "application/pdf",
                $"ReporteEnsayosCliente_{DateTime.Now:g}.pdf"
            );
        }
    }
}