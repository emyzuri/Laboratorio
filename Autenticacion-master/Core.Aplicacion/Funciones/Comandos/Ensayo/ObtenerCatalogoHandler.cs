using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ObtenerCatalogoHandler : IRequestHandler<ObtenerCatalogoCom, IEnumerable<CatalogoEnsayoModel>>
    {
        private readonly IEnsayo _iEnsayo;
        public ObtenerCatalogoHandler(IEnsayo iEnsayo)
        {
            _iEnsayo = iEnsayo;
        }
        public async Task<IEnumerable<CatalogoEnsayoModel>> Handle(ObtenerCatalogoCom request, CancellationToken cancellationToken)
        {
            return await _iEnsayo.ObtenerCatalogoEnsayo();
        }
    }
}
