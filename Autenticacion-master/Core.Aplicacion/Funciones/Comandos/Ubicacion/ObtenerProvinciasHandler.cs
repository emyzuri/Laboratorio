using Core.Aplicacion.RespuestaUtilitario;
using Core.DataAccess.Clientes.Interfaz;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Ubicacion
{
    public class ObtenerProvinciasHandler : IRequestHandler<ObtenerProvinciasCom, Respuesta>
    {
        private readonly IUbicacion _iUbicacion;

        public ObtenerProvinciasHandler(IUbicacion iUbicacion)
        {
            _iUbicacion = iUbicacion;
        }

        public async Task<Respuesta> Handle(ObtenerProvinciasCom request, CancellationToken cancellationToken)
        {
            // Obtienes la lista del servicio
            var lista = (await _iUbicacion.ObtenerProvincias()).ToList();

            // Retornas el objeto Respuesta que espera tu controlador
            return new Respuesta
            {
                EsExitoso = true,
                Datos = lista
            };
        }
    }
}