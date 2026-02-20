using Core.Aplicacion.RespuestaUtilitario;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Clientes;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class CrearClienteHandler : IRequestHandler<CrearClienteCom, CrearClienteModel>
    {
        private readonly ICliente _clienteServicio;
        private readonly ICacheServicio _cacheServicio;

        public CrearClienteHandler(ICliente clienteServicio, ICacheServicio cacheServicio)
        {
            _clienteServicio = clienteServicio;
            _cacheServicio = cacheServicio;
        }

        public async Task<CrearClienteModel> Handle(CrearClienteCom request, CancellationToken cancellationToken)
        {
            ClienteModel nuevoCliente = new()
            {
                Cedula = request.Cedula,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Ciudad = request.Ciudad,
                Titulo = request.Titulo
            };

            var clienteBd = await _clienteServicio.ConsultarCliente(new ConsultarClienteModel { Cedula = request.Cedula });
            if (clienteBd != null)
            {
                if (clienteBd.Estado)
                {
                    throw new ManejoExcepciones("Ya existe un cliente con la cédula proporcionada.");
                }
                nuevoCliente.IdCliente = clienteBd.IdCliente;
                await _clienteServicio.ActualizarCliente(nuevoCliente);
                await _clienteServicio.ActivarCliente(clienteBd.Cedula);
                return clienteBd;
            }
            return await _clienteServicio.InsertarCliente(nuevoCliente);

            //await _cacheServicio.Agregar("UltimoClienteId", idGenerado.ToString(), new TimeSpan(0, 5, 0));
        }
    }
}