using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    /// <summary>
    /// Clase transaccional para consultar ensayos de un cliente específico.
    /// </summary>
    /// <remarks>
    /// Constructor que inicializa la cédula del cliente y las fechas de inicio y fin para la consulta de ensayos. Estos parámetros son esenciales para identificar al cliente y establecer el rango de tiempo en el cual se desean obtener los ensayos realizados por dicho cliente.
    /// </remarks>
    /// <param name="cedula">Cedula del cliente para el cual se desean consultar los ensayos.</param>
    /// <param name="fechaInicio">Fecha inicio filtro para la consulta de ensayos.</param>
    /// <param name="fechaFin">Fecha fin filtro para la consulta de ensayos.</param>
    public class ConsultarEnsayosClienteCom(string cedula, DateTime fechaInicio, DateTime fechaFin) : IRequest<IEnumerable<ClienteDeudorModel>>
    {
        /// <summary>
        /// Cedula del cliente para el cual se desean consultar los ensayos. Este campo es esencial para identificar al cliente en la base de datos y obtener la información relevante sobre sus ensayos dentro del rango de fechas especificado.
        /// </summary>
        public string Cedula { get; set; } = cedula;

        /// <summary>
        /// Fecha inicio filtro para la consulta de ensayos. Este campo se utiliza para establecer el límite inferior del rango de fechas en el cual se desean obtener los ensayos realizados por el cliente. Es fundamental para asegurar que solo se recuperen los ensayos que se encuentran dentro del período de tiempo especificado.
        /// </summary>
        public DateTime FechaInicio { get; set; } = fechaInicio;

        /// <summary>
        /// Fecha fin filtro para la consulta de ensayos. Este campo se utiliza para establecer el límite superior del rango de fechas en el cual se desean obtener los ensayos realizados por el cliente. Es fundamental para asegurar que solo se recuperen los ensayos que se encuentran dentro del período de tiempo especificado.
        /// </summary>
        public DateTime FechaFin { get; set; } = fechaFin;
    }
}