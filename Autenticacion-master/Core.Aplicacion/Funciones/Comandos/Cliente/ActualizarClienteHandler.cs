using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    /// <summary>
    /// Logica de actualizacion de cliente
    /// </summary>
    public class ActualizarClienteHandler(ICliente iCliente, ICacheServicio iCacheServicio) : IRequestHandler<ActualizarClienteCom, Unit>
    {
        /// <summary>
        /// Servicio de cliente
        /// </summary>
        private readonly ICliente iCliente = iCliente ?? throw new ArgumentException(nameof(iCliente));

        /// <summary>
        /// Servicio de cache
        /// </summary>
        private readonly ICacheServicio iCacheServicio = iCacheServicio;

        /// <summary>
        /// Logica de actualizacion de cliente
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Actualizacion</returns>
        public async Task<Unit> Handle(ActualizarClienteCom request, CancellationToken cancellationToken)
        {
            IEnumerable<ClienteModel> clientes = await iCacheServicio.Obtener<IEnumerable<ClienteModel>>("Clientes_");
            ClienteModel cliente = clientes.FirstOrDefault(c => c.IdCliente == request.IdCliente);
            
            if (cliente == null)
            {
                throw new ManejoExcepciones("No se encontró un cliente con el ID proporcionado.");
            }

            clientes = clientes.Where(c => c.IdCliente != request.IdCliente);
            cliente.Nombre = request.Nombre;
            cliente.Apellido = request.Apellido;
            cliente.Telefono = request.Telefono;
            cliente.Direccion = request.Direccion;
            cliente.Ciudad = request.Ciudad;
            cliente.Titulo = request.Titulo;
            cliente.Correo = request.Correo;
            clientes = clientes.Append(cliente);

            ClienteModel clienteParaActualizar = new()
            {
                IdCliente = request.IdCliente,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Ciudad = request.Ciudad,
                Titulo = request.Titulo,
                Correo = request.Correo,
            };

            await iCliente.ActualizarCliente(clienteParaActualizar);
            await iCacheServicio.Agregar($"Clientes_", clientes, TimeSpan.FromMinutes(480));
            return Unit.Value;
        }
    }
}