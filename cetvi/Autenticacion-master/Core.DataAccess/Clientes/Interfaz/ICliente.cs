// ICliente.cs
using Core.Dominio.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface ICliente
    {
        Task<ClienteModel> ObtenerCliente(int idCliente);
        Task<bool> DesactivarCliente(int idCliente);
        Task<bool> ActualizarCliente(ClienteModel cliente);
        Task<List<ClienteModel>> ConsultarClientes();
        Task<int> InsertarCliente(ClienteModel cliente);
    }
}