using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    internal class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCom, bool>
    {
        private readonly IUsuario _iUsuario;

        public RegistrarUsuarioHandler(IUsuario iUsuario)
        {
            _iUsuario = iUsuario ?? throw new ArgumentNullException(nameof(iUsuario));
        }

        public async Task<bool> Handle(RegistrarUsuarioCom request, CancellationToken cancellationToken)
        {
            if (request.Roles == null || request.Roles.Count == 0)
                throw new Exception("Debe asignar al menos un rol.");

            var nuevoUsuario = new UsuarioModel
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Usuario = request.Usuario,
                Password = request.Password,
                Telefono = request.Telefono,
                Cedula = request.Cedula
            };

            return await _iUsuario.RegistrarUsuario(nuevoUsuario, request.Roles);
        }
    }

}