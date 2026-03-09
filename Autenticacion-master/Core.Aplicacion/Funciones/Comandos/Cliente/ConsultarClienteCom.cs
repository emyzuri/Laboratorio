using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    /// <summary>
    /// Clase transaccional para obtener clientes.
    /// </summary>
    public class ConsultarClienteCom : IRequest<IEnumerable<ClienteModel>>
    {
    }
}
