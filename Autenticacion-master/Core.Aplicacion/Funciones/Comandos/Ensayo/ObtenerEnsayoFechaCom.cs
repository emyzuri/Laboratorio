
using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    /// <summary>
    /// Clase transaccional para obtener ensayos por fecha.
    /// </summary>
    /// <remarks>
    /// Constructor que inicializa las fechas de inicio y fin para la consulta de ensayos. Estas fechas se utilizan como filtros para obtener los ensayos realizados dentro de ese rango de tiempo.
    /// </remarks>
    /// <param name="fechaInicio">Fecha inicio filtro para la consulta de ensayos.</param>
    /// <param name="fechaFin">Fecha fin filtro para la consulta de ensayos.</param>
    public class ObtenerEnsayoFechaCom(DateTime fechaInicio, DateTime fechaFin) : IRequest<IEnumerable<ClienteDeudorModel>>
    {
        /// <summary>
        /// Fecha inicio filtro para la consulta de ensayos.
        /// </summary>
        public DateTime FechaInicio { get; set; } = fechaInicio;

        /// <summary>
        /// Fecha fin filtro para la consulta de ensayos.
        /// </summary>
        public DateTime FechaFin { get; set; } = fechaFin;
    }
}
