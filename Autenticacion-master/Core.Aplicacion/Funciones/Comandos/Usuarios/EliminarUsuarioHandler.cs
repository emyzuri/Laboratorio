using Core.DataAccess.Clientes.Interfaz;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class EliminarUsuarioHandler : IRequestHandler<EliminarUsuarioCom, bool>
    {
        private readonly IUsuario _usuarioServicio;

        public EliminarUsuarioHandler(IUsuario usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public async Task<bool> Handle(EliminarUsuarioCom request, CancellationToken cancellationToken)
        {
            return await _usuarioServicio.EliminarUsuario(request.IdUsuario);
        }
    }
}