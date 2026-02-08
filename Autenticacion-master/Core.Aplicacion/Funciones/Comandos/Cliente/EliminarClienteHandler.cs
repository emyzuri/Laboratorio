using Core.DataAccess.Clientes.Interfaz;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class EliminarClienteHandler : IRequestHandler<EliminarClienteCom, Unit>
    {
        private readonly ICliente _clienteServicio;

        public EliminarClienteHandler(ICliente clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        public async Task<Unit> Handle(EliminarClienteCom request, CancellationToken cancellationToken)
        {
            await _clienteServicio.DesactivarCliente(request.IdCliente);
            return Unit.Value;
        }
    }
}