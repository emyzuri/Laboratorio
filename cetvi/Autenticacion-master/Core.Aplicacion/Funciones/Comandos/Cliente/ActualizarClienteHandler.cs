using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Aplicacion.RespuestaUtilitario;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ActualizarClienteHandler : IRequestHandler<ActualizarClienteCom, Respuesta<bool>>
    {
        private readonly ICliente iCliente;

        public ActualizarClienteHandler(ICliente iCliente)
        {
            this.iCliente = iCliente ?? throw new ArgumentException(nameof(iCliente));
        }

        public async Task<Respuesta<bool>> Handle(ActualizarClienteCom request, CancellationToken cancellationToken)
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

            bool resultado = await iCliente.ActualizarCliente(clienteParaActualizar);

            return new Respuesta<bool>(resultado);
        }
    }
}