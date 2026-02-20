using MediatR;
using Core.Dominio.Model;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class RegistrarUsuarioCom : IRequest<bool>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; } 
        public string Password { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public List<int> Roles { get; set; } = new();


    }
}