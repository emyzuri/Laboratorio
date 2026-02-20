
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Roles
{
    public class ListarPermisosCom : IRequest<List<RolModel>> { }
    public class QuitarPermisoCom : IRequest<bool>
    {
        public int IdUsuarioRol { get; set; } 
    }
}
