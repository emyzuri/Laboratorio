using MediatR;
using Core.Dominio.Model;
using System.Collections.Generic;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class ConsultarRolesCom : IRequest<List<RolModel>>
    {
    }
}