using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    /// <summary>
    /// Logica de eliminacion de cliente.
    /// </summary>
    public class EliminarClienteHandler(ICliente clienteServicio, ICacheServicio iCacheServicio) : IRequestHandler<EliminarClienteCom, Unit>
    {
        /// <summary>
        /// Servicio de cliente
        /// </summary>
        private readonly ICliente _clienteServicio = clienteServicio;

        /// <summary>
        /// Servicio de cache
        /// </summary>
        private readonly ICacheServicio iCacheServicio = iCacheServicio;

        /// <summary>
        /// Logica de eliminacion de cliente
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns></returns>
        /// <exception cref="ManejoExcepciones">Control de excepciones</exception>
        public async Task<Unit> Handle(EliminarClienteCom request, CancellationToken cancellationToken)
        {
            IEnumerable<ClienteModel> clientes = await iCacheServicio.Obtener<IEnumerable<ClienteModel>>("Clientes_");
            ClienteModel cliente = clientes.FirstOrDefault(c => c.IdCliente == request.IdCliente);

            if (cliente == null)
            {
                throw new ManejoExcepciones("No se encontró un cliente con el ID proporcionado.");
            }

            clientes = clientes.Where(c => c.IdCliente != request.IdCliente);
            await _clienteServicio.DesactivarCliente(request.IdCliente);
            await iCacheServicio.Agregar($"Clientes_", clientes, TimeSpan.FromMinutes(480));
            return Unit.Value;
        }
    }
}