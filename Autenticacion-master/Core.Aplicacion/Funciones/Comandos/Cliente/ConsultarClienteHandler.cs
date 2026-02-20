using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class ConsultarClienteHandler : IRequestHandler<ConsultarClienteCom, IEnumerable<ClienteModel>>
    {
        private readonly ICliente iCliente;
        readonly ICacheServicio cacheServicio;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ConsultarClienteHandler(ICliente iCliente, ICacheServicio cacheServicio, IHttpContextAccessor httpContextAccessor)
        {
            this.iCliente = iCliente ?? throw new ArgumentException(nameof(iCliente));
            this.cacheServicio = cacheServicio ?? throw new ArgumentException(nameof(cacheServicio));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<ClienteModel>> Handle(ConsultarClienteCom request, CancellationToken cancellationToken)
        {
            ClienteModel usuario = await cacheServicio.Obtener<ClienteModel>(httpContextAccessor.HttpContext.Request.Headers["IdSesion"]);
            if (usuario == null)
            {
                throw new ArgumentException("Sesión caducada");
            }
            return await iCliente.ConsultarClientes();
        }
    }
}