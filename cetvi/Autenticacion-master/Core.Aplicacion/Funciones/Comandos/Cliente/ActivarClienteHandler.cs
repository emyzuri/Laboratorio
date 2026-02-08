
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ActivarClienteHandler : IRequestHandler<ActivarClienteCom, Unit>
    {
        private readonly ICliente iCliente;

        public ActivarClienteHandler(ICliente iCliente)
        {
            this.iCliente = iCliente ?? throw new ArgumentException(nameof(iCliente));
        }
        public async Task<Unit> Handle(ActivarClienteCom request, CancellationToken cancellationToken)
        {
            await iCliente.ActivarCliente(request.Cedula);
            return Unit.Value;
        }


    }
}
