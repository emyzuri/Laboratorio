using Core.Aplicacion.RespuestaUtilitario;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ObtenerCatalogoCom : IRequest<Respuesta>
    {
        public int? IdPadre { get; set; }

        public ObtenerCatalogoCom(int? idPadre = 0)
        {
            IdPadre = idPadre;
        }
    }
}