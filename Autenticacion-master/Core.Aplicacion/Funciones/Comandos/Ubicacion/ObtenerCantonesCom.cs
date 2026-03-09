using Core.Dominio.Model.Ubicacion;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ubicacion
{
    public class ObtenerCantonesCom : IRequest<List<CantonModel>>
    {
        public int IdProvincia { get; set; }
        public ObtenerCantonesCom(int idProvincia) => IdProvincia = idProvincia;
    }
}
