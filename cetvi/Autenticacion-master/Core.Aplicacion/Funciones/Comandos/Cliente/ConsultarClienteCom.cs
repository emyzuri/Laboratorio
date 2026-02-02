using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Usuarios
{
    public class ConsultarClienteCom : IRequest<IEnumerable<ClienteModel>>
    {
    }
}
