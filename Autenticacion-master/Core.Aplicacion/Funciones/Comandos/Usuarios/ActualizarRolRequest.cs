using MediatR;
using System.Collections.Generic;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class ActualizarRolRequest : IRequest<bool>
    {
        public int IdUsuario { get; set; }
        public List<int> Roles { get; set; } = new();
    }
}
