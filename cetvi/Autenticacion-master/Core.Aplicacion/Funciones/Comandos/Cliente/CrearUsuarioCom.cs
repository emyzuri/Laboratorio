using Core.Aplicacion.RespuestaUtilitario;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class CrearUsuarioCom : IRequest<Respuesta<int>>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaActualizacion { get; set; }
        public int NumeroLogin { get; set; }
        public string Activo { get; set; }

    }
}
