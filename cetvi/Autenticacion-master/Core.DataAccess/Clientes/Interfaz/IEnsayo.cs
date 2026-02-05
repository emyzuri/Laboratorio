
using Core.Dominio.Model;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface IEnsayo
    {
        Task InsertarEnsayo(EnsayoModel ensayo, int idCliente, int idPrueba, string usuario);
        Task<int> ConsultarUltimoIdPrueba();
    }
}
