using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.Aplicacion.RespuestaUtilitario;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util; // Asegúrate de tener este namespace para ICacheServicio
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    public class ObtenerCatalogoHandler : IRequestHandler<ObtenerCatalogoCom, Respuesta>
    {
        private readonly IEnsayo _iEnsayo;
        private readonly ICacheServicio _cacheServicio;

        /// <summary>
        /// Constructor de la clase inyectando el servicio de cache como la guía
        /// </summary>
        public ObtenerCatalogoHandler(IEnsayo iEnsayo, ICacheServicio cacheServicio)
        {
            _iEnsayo = iEnsayo ?? throw new ArgumentException(nameof(iEnsayo));
            _cacheServicio = cacheServicio ?? throw new ArgumentException(nameof(cacheServicio));
        }

        public async Task<Respuesta> Handle(ObtenerCatalogoCom request, CancellationToken cancellationToken)
        {
            var datos = await _iEnsayo.ObtenerCatalogoEnsayo(request.IdPadre);

            return new Respuesta
            {
                EsExitoso = true,
                Datos = datos
            };
        }
    }
}