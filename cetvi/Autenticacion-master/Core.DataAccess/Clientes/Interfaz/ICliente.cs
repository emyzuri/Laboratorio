// ICliente.cs
using Core.Dominio.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface ICliente
    {
        Task<ClienteModel> ObtenerCliente(int idCliente);
        Task DesactivarCliente(int idCliente);
        Task ActualizarCliente(ClienteModel cliente);
        Task<IEnumerable<ClienteModel>> ConsultarClientes();
        Task InsertarCliente(ClienteModel cliente);
    }
}