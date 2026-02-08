using Core.Dominio.Clientes;
using Core.Dominio.Model;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface ICliente
    {
        Task<ClienteModel> ObtenerCliente(int idCliente);
        Task DesactivarCliente(int idCliente);
        Task ActualizarCliente(ClienteModel cliente);
        Task<IEnumerable<ClienteModel>> ConsultarClientes();
        Task<CrearClienteModel> InsertarCliente(ClienteModel cliente);
        Task<CrearClienteModel> ConsultarCliente(ConsultarClienteModel cliente);
        Task ActivarCliente(string cedula);
    }
}