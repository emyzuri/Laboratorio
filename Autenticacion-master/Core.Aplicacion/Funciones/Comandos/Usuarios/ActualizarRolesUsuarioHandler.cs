using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    internal class ActualizarRolesUsuarioHandler
        : IRequestHandler<ActualizarRolRequest, bool>
    {
        private readonly IUsuario _iUsuario;

        public ActualizarRolesUsuarioHandler(IUsuario iUsuario)
        {
            _iUsuario = iUsuario ?? throw new ArgumentNullException(nameof(iUsuario));
        }

        public async Task<bool> Handle(ActualizarRolRequest request, CancellationToken cancellationToken)
        {
            return await _iUsuario.ActualizarRolesUsuario(request.IdUsuario, request.Roles);
        }
    }
}