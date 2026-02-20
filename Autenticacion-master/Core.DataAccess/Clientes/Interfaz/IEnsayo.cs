using Core.Dominio.Model;
using Core.Dominio.Request.Ensayos;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface IEnsayo
    {
        Task InsertarEnsayo(EnsayoModel ensayo, int idCliente, int idPrueba, string usuario);
        Task<int> ConsultarUltimoIdPrueba();
        Task RegistrarPago(int idCliente, decimal abono, decimal montoTotal, string usuario, int idPrueba, DateTime fechaEntrega,string descripcion);
        Task<IEnumerable<ConsultarAbonoRequest>> ObtenerAbonosPorCliente(int idCliente);
        Task<IEnumerable<PagoModel>> ObtenerPagosPorPrueba(int idPrueba);
        Task<IEnumerable<ClienteDeudorModel>> ObtenerClientesDeudores();
        Task<IEnumerable<CatalogoEnsayoModel>> ObtenerCatalogoEnsayo();
        Task<bool> RegistrarNuevoAbono(int idEnsayo, decimal monto, string usuario);
        Task<IEnumerable<EnsayoDetalladoModel>> ObtenerEnsayosDetallados(int idPrueba);
    }
}