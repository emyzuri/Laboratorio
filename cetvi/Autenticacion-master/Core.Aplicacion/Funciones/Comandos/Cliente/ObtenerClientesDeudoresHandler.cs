
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class ObtenerClientesDeudoresHandler : IRequestHandler<ObtenerClientesDeudoresCom, IEnumerable<ClienteDeudorModel>>
    {
        private readonly IEnsayo _iEnsayo;
        public ObtenerClientesDeudoresHandler(IEnsayo iEnsayo)
        {
            _iEnsayo = iEnsayo;
        }
        public async Task<IEnumerable<ClienteDeudorModel>> Handle(ObtenerClientesDeudoresCom request, CancellationToken cancellationToken)
        {
            return await _iEnsayo.ObtenerClientesDeudores();
        }
    }
}
