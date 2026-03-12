using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class ActualizarUsuarioCom : IRequest<bool>
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }

        public ActualizarUsuarioCom() { }
    }
}