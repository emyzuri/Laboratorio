using Core.Aplicacion.RespuestaUtilitario;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class CrearClienteHandler : IRequestHandler<CrearClienteCom, Respuesta<int>>
    {
        private readonly ICliente _clienteServicio;
        private readonly ICacheServicio _cacheServicio;

        public CrearClienteHandler(ICliente clienteServicio, ICacheServicio cacheServicio)
        {
            _clienteServicio = clienteServicio;
            _cacheServicio = cacheServicio;
        }

        public async Task<Respuesta<int>> Handle(CrearClienteCom request, CancellationToken cancellationToken)
        {
            var nuevoCliente = new ClienteModel
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Ciudad = request.Ciudad,
                Titulo = request.Titulo
            };

            int idGenerado = await _clienteServicio.InsertarCliente(nuevoCliente);

            await _cacheServicio.Agregar("UltimoClienteId", idGenerado.ToString(), new TimeSpan(0, 5, 0));

            return new Respuesta<int>(idGenerado);
        }
    }
}