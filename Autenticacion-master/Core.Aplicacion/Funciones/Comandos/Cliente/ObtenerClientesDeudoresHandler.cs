
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
            IEnumerable<ClienteDeudorModel> clientes = await _iEnsayo.ObtenerClientesDeudores();
            if (clientes != null)
            {
                foreach (var cliente in clientes)
                {
                    cliente.Ensayos = await _iEnsayo.ObtenerEnsayosDetallados(cliente.IdEnsayo);
                    cliente.NombreCompleto = cliente.Ensayos.FirstOrDefault().NombreCompleto;
                    cliente.IdCliente = cliente.Ensayos.FirstOrDefault().IdCliente;
                    cliente.Cedula = cliente.Ensayos.FirstOrDefault().Cedula;
                }
            }
            return clientes;
        }
    }
}
