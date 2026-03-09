using Core.Aplicacion.RespuestaUtilitario;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Clientes;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    /// <summary>
    /// Logica de creacion de cliente
    /// </summary>
    public class CrearClienteHandler(ICliente clienteServicio, ICacheServicio iCacheServicio) : IRequestHandler<CrearClienteCom, ClienteModel>
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
        /// Logica de creacion de cliente
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Cliente</returns>
        /// <exception cref="ManejoExcepciones">manejo de excepciones</exception>
        public async Task<ClienteModel> Handle(CrearClienteCom request, CancellationToken cancellationToken)
        {
            IEnumerable<ClienteModel> clientes = await iCacheServicio.Obtener<IEnumerable<ClienteModel>>("Clientes_");
            if (clientes != null && clientes.Any(c => c.Cedula == request.Cedula))
            {
                throw new ManejoExcepciones("Ya existe un cliente con la cédula proporcionada.");
            }

            ClienteModel nuevoCliente = new()
            {
                Cedula = request.Cedula,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Ciudad = request.Ciudad,
                Titulo = request.Titulo,
                Correo = request.Correo,
            };

            ClienteModel cliente =  await _clienteServicio.InsertarCliente(nuevoCliente);
            clientes = clientes.Append(cliente);
            await iCacheServicio.Agregar($"Clientes_", clientes, TimeSpan.FromMinutes(480));
            return cliente;

        }
    }
}