using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Usuario
{
    public class ValidarUsuarioCom : IRequest<UsuarioModel>
    {
        public string password { get; set; }
        public string usuario { get; set; }
        /// <param name="Password">Clave del usuario</param>
        /// <param name="Usuario">Usuario</param>
        public ValidarUsuarioCom(string Password, string Usuario)
        {
            this.password = Password;
            this.usuario = Usuario;
        }
    }
}
