using Core.DataAccess.Clientes.Interfaz;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class EliminarClienteHandler : IRequestHandler<EliminarClienteCom, bool>
    {
        private readonly ICliente _clienteServicio;

        public EliminarClienteHandler(ICliente clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        public async Task<bool> Handle(EliminarClienteCom request, CancellationToken cancellationToken)
        {
            return await _clienteServicio.DesactivarCliente(request.IdCliente);
        }
    }
}