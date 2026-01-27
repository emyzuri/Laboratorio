using Core.Dominio.Comunes;

namespace Core.Dominio.Model
{
    public class UsuarioModel 
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public Guid IdSesion { get; set; }

        public UsuarioModel()
        {
            IdSesion = Guid.NewGuid();
        }
    }
}
