using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ObtenerEnsayoHandler : IRequestHandler<ObtenerEnsayoCom, IEnumerable<EnsayoDetalladoModel>>
    {
        private readonly IEnsayo _ensayoRepo;

        public ObtenerEnsayoHandler(IEnsayo ensayoRepo)
        {
            _ensayoRepo = ensayoRepo;
        }

        public async Task<IEnumerable<EnsayoDetalladoModel>> Handle(ObtenerEnsayoCom request, CancellationToken cancellationToken)
        {
            return await _ensayoRepo.ObtenerEnsayosDetallados(request.IdPrueba);
        }
    }
}