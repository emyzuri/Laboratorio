using Core.Dominio.Model;
using Core.Dominio.Request.Ensayos;
using System.Threading.Tasks;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface IEnsayo
    {
        Task InsertarEnsayo(EnsayoModel ensayo, int idCliente, int idPrueba, string usuario);
        Task<int> ConsultarUltimoIdPrueba();
        Task RegistrarPago(int idCliente, decimal abono, decimal montoTotal, string usuario, int idPrueba, DateTime fechaEntrega,string descripcion);
        Task<IEnumerable<ConsultarAbonoRequest>> ObtenerAbonosPorCliente(int idCliente);
        Task<IEnumerable<PagoModel>> ObtenerPagosPorPrueba(int idPrueba);

        /// <summary>
        /// Consulta de clientes deudores, es decir, aquellos que tienen pagos pendientes o deudas relacionadas con los ensayos realizados. Esta consulta puede incluir información como el nombre del cliente, el monto adeudado, la fecha del último pago y detalles de los ensayos asociados a la deuda.
        /// </summary>
        /// <returns>Lista de clientes deudores</returns>
        Task<IEnumerable<ClienteDeudorModel>> ObtenerClientesDeudores();
        Task<IEnumerable<CatalogoEnsayoModel>> ObtenerCatalogoEnsayo(int? idPadre = null);
        Task<IEnumerable<CatalogoEnsayoModel>> ObtenerCatalogoJerarquico();
        Task<bool> RegistrarNuevoAbono(int idEnsayo, decimal monto, string usuario);

        /// <summary>
        /// Consulta detallada de los ensayos realizados para una prueba específica, incluyendo información del catálogo y detalles relevantes.
        /// </summary>
        /// <param name="idPrueba">Identificador del ensayo</param>
        /// <returns>Lista de ensayos realizados</returns>
        Task<IEnumerable<EnsayoDetalladoModel>> ObtenerEnsayosDetallados(int idPrueba);
        Task<IEnumerable<ReporteEnsayoModel>> ObtenerReportePorFecha(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// COnsulta de ensayos por rango de fechas. Permite filtrar los ensayos realizados dentro de un período determinado, facilitando la generación de reportes y análisis de datos históricos. Esta consulta puede incluir información como el nombre del cliente, el monto adeudado, la fecha del último pago y detalles de los ensayos asociados a la deuda, además de convertir el nombre completo a mayúsculas para estandarizar la presentación.
        /// </summary>
        /// <param name="fechaInicio">Fecha inicio del filtro</param>
        /// <param name="fechaFin">Fecha fin del filtro</param>
        /// <returns>LIsta de ensayos</returns>
        Task<IEnumerable<ClienteDeudorModel>> ConsultarEnsayoFechas(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<ClienteDeudorModel>> ConsultarDeudaEnsayo(int idPrueba);

        /// <summary>
        /// Consulta de ensayos por cédula del cliente. Permite obtener una lista de clientes deudores que han realizado ensayos dentro del rango de fechas especificado, y se enriquecen con los detalles de sus ensayos asociados, además de convertir el nombre completo a mayúsculas para estandarizar la presentación.
        /// </summary>
        /// <param name="cedula">Identificacion del cliente</param>
        /// <param name="fechaInicio">Fecha inicio del filtro</param>
        /// <param name="fechaFin">Fecha fin del filtro</param>
        /// <returns>Lista de ensayos</returns>
        Task<IEnumerable<ClienteDeudorModel>> ConsultarPorCedula(string cedula, DateTime fechaInicio, DateTime fechaFin);
    }
}