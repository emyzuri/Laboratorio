using Core.Dominio.Model;
using Core.Dominio.Request.Ensayos;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface IEnsayo
    {
        Task InsertarEnsayo(EnsayoModel ensayo, int idCliente, int idPrueba, string usuario);
        Task<int> ConsultarUltimoIdPrueba();
        Task RegistrarPago(int idCliente, double abono, double montoTotal, string usuario, int idPrueba);
        Task<IEnumerable<ConsultarAbonoRequest>> ObtenerAbonosPorCliente(int idCliente);
        Task<IEnumerable<PagoModel>> ObtenerPagosPorPrueba(int idPrueba);
        Task<IEnumerable<ClienteDeudorModel>> ObtenerClientesDeudores();
    }
}