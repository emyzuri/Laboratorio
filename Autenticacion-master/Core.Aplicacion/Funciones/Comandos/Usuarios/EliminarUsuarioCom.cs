using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class EliminarUsuarioCom : IRequest<bool>
    {
        public int IdUsuario { get; set; }

        public EliminarUsuarioCom(int idUsuario)
        {
            IdUsuario = idUsuario;
        }
    }
}