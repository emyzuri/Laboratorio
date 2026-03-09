
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    /// <summary>
    /// Logica de negocio para obtener clientes deudores.
    /// </summary>
    public class ObtenerClientesDeudoresHandler : IRequestHandler<ObtenerClientesDeudoresCom, IEnumerable<ClienteDeudorModel>>
    {
        /// <summary>
        /// Servicio de acceso a datos para ensayos, utilizado para obtener información de clientes deudores y sus ensayos asociados.
        /// </summary>
        private readonly IEnsayo _iEnsayo;

        /// <summary>
        /// Constructor que inyecta el servicio de acceso a datos para ensayos.
        /// </summary>
        /// <param name="iEnsayo">Servicio de acceso a datos para ensayos</param>
        public ObtenerClientesDeudoresHandler(IEnsayo iEnsayo)
        {
            _iEnsayo = iEnsayo;
        }

        /// <summary>
        /// Logica de negocio para manejar la solicitud de obtener clientes deudores. Se obtiene la lista de clientes deudores y se enriquecen con los detalles de sus ensayos asociados, además de convertir el nombre completo a mayúsculas para estandarizar la presentación.
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Lista de clientes deudores</returns>
        public async Task<IEnumerable<ClienteDeudorModel>> Handle(ObtenerClientesDeudoresCom request, CancellationToken cancellationToken)
        {
            IEnumerable<ClienteDeudorModel> clientes = await _iEnsayo.ObtenerClientesDeudores();
            if (clientes != null)
            {
                foreach (var item in clientes)
                {
                    item.Ensayos = await _iEnsayo.ObtenerEnsayosDetallados(item.IdEnsayo);
                    item.NombreCompleto = item.NombreCompleto?.ToUpper();
                }
            }

            return clientes ?? [];

        }
    }
}
