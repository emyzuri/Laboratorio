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
    public class ConsultarClienteHandler : IRequestHandler<ConsultarClienteCom, List<ClienteModel>>
    {
        private readonly ICliente iCliente;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ConsultarClienteHandler(ICliente iCliente, IHttpContextAccessor httpContextAccessor)
        {
            this.iCliente = iCliente ?? throw new ArgumentException(nameof(iCliente));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        }

        public async Task<List<ClienteModel>> Handle(ConsultarClienteCom request, CancellationToken cancellationToken)
        {
            var idSesion = httpContextAccessor.HttpContext.Request.Headers["IdSesion"].ToString();
            return await iCliente.ConsultarClientes();
        }
    }
}