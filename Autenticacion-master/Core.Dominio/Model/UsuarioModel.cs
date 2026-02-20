using Core.Dominio.Comunes;
using System.Text.Json.Serialization;

namespace Core.Dominio.Model
{
    public class UsuarioModel 
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public Guid IdSesion { get; set; }
        public int IdRol { get; set; }
        public string Roles { get; set; }

        public UsuarioModel()
        {
            IdSesion = Guid.NewGuid();
        }
    }
}
