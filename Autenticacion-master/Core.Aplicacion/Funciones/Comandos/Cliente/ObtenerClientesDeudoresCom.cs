using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    /// <summary>
    /// Modelo transaccional para obtener clientes deudores.
    /// </summary>
    public class ObtenerClientesDeudoresCom : IRequest<IEnumerable<ClienteDeudorModel>>
    {
    }
}
