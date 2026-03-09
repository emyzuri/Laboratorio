using MediatR;
using Core.Aplicacion.RespuestaUtilitario;
using Core.Dominio.Model;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ObtenerEnsayoCom : IRequest<IEnumerable<EnsayoDetalladoModel>>
    {
        public int IdPrueba { get; set; }

        public ObtenerEnsayoCom(int idPrueba)
        {
            this.IdPrueba = idPrueba;
        }
    }
}
