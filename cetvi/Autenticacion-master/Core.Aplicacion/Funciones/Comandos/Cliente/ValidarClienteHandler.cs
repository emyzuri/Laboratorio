using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ValidarClienteHandler : IRequestHandler<ValidarClienteCom, ClienteModel>
    {
        private readonly ICliente iCliente;
        private readonly ICacheServicio cacheServicio;

        public ValidarClienteHandler(ICliente iCliente, ICacheServicio cacheServicio)
        {
            this.iCliente = iCliente ?? throw new ArgumentException(nameof(iCliente));
            this.cacheServicio = cacheServicio ?? throw new ArgumentException(nameof(cacheServicio));
        }

        public async Task<ClienteModel> Handle(ValidarClienteCom request, CancellationToken cancellationToken)
        {
            ClienteModel cliente = await iCliente.ObtenerCliente(request.IdCliente);

            if (cliente != null)
            {
                await cacheServicio.Agregar("cliente_" + cliente.IdCliente.ToString(), cliente, new TimeSpan(0, 6, 0));
            }

            return cliente;
        }
    }
}