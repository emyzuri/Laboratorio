using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class ActualizarUsuarioHandler : IRequestHandler<ActualizarUsuarioCom, bool>
    {
        private readonly IUsuario _usuarioServicio;

        public ActualizarUsuarioHandler(IUsuario usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public async Task<bool> Handle(ActualizarUsuarioCom request, CancellationToken cancellationToken)
        {
            var usuarioModel = new UsuarioModel
            {
                IdUsuario = request.IdUsuario,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Usuario = request.Usuario,
                Telefono = request.Telefono,
                Cedula = request.Cedula
            };

            return await _usuarioServicio.ActualizarUsuario(usuarioModel);
        }
    }
}