using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    internal class ConsultarRolesHandler : IRequestHandler<ConsultarRolesCom, List<RolModel>>
    {
        private readonly IUsuario _usuarioServicio;

        public ConsultarRolesHandler(IUsuario usuarioServicio)
        {
            _usuarioServicio = usuarioServicio ?? throw new ArgumentNullException(nameof(usuarioServicio));
        }

        public async Task<List<RolModel>> Handle(ConsultarRolesCom request, CancellationToken cancellationToken)
        {
            return await _usuarioServicio.ObtenerRoles();
        }
    }
}