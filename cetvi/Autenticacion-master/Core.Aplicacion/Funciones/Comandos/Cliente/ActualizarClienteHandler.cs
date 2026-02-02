using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ActualizarClienteHandler : IRequestHandler<ActualizarClienteCom, Unit>
    {
        private readonly ICliente iCliente;

        public ActualizarClienteHandler(ICliente iCliente)
        {
            this.iCliente = iCliente ?? throw new ArgumentException(nameof(iCliente));
        }

        public async Task<Unit> Handle(ActualizarClienteCom request, CancellationToken cancellationToken)
        {
            ClienteModel clienteParaActualizar = new()
            {
                IdCliente = request.IdCliente,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Ciudad = request.Ciudad,
                Titulo = request.Titulo
            };

            await iCliente.ActualizarCliente(clienteParaActualizar);

            return Unit.Value;
        }
    }
}