using Core.Dominio;
using Core.Dominio.Request.Ensayos;
using MediatR;
using System.Collections.Generic;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class ActualizarRolesUsuarioCom : IRequest<IEnumerable<ActualizarRolRequest>>
    { 
    }
}
