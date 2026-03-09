using Core.Aplicacion.RespuestaUtilitario;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ubicacion
{
    public class ObtenerParroquiasCom : IRequest<Respuesta>
    {
        public int IdCanton { get; set; }
        public ObtenerParroquiasCom(int idCanton) => IdCanton = idCanton;
    }
}