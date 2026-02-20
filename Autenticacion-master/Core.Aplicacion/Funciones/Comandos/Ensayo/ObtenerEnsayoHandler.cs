using Core.DataAccess.Clientes.Interfaz;
using MediatR;
namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ObtenerEnsayoHandler : IRequestHandler<ObtenerEnsayoCom, object>
    {
        private readonly IEnsayo _ensayoRepo;

        public ObtenerEnsayoHandler(IEnsayo ensayoRepo)
        {
            _ensayoRepo = ensayoRepo;
        }

        public async Task<object> Handle(ObtenerEnsayoCom request, CancellationToken cancellationToken)
        {
            return await _ensayoRepo.ObtenerEnsayosDetallados(request.IdPrueba);
        }
    }
}
