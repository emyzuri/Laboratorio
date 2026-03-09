
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using Polly;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    /// <summary>
    /// Logica de negocio para obtener ensayos por fecha.
    /// </summary>
    /// <remarks>
    /// Constructor que inyecta el servicio de acceso a datos para ensayos.
    /// </remarks>
    /// <param name="iEnsayo">Servicio de acceso a datos para ensayos</param>
    internal class ObtenerEnsayoFechaHandler(IEnsayo iEnsayo, ICacheServicio cacheServicio, IHttpContextAccessor context) : IRequestHandler<ObtenerEnsayoFechaCom, IEnumerable<ClienteDeudorModel>>
    {
        /// <summary>
        /// Servicio de acceso a datos para ensayos, utilizado para obtener información de clientes deudores y sus ensayos asociados.
        /// </summary>
        private readonly IEnsayo _iEnsayo = iEnsayo;

        /// <summary>
        /// Servicio de cache utilizado para almacenar temporalmente los resultados de las consultas, mejorando el rendimiento y reduciendo la carga en la base de datos al evitar consultas repetitivas para la misma información dentro de un período de tiempo determinado. Este servicio se inyecta a través del constructor para facilitar la separación de responsabilidades y mejorar la testabilidad de la clase.
        /// </summary>
        private readonly ICacheServicio _iCacheServicio = cacheServicio;

        /// <summary>
        /// HttpContextAccessor utilizado para acceder al contexto HTTP actual, lo que permite obtener información relevante de la solicitud, como los encabezados, que pueden ser útiles para la gestión de sesiones o para personalizar la respuesta según el usuario o la sesión. Este servicio se inyecta a través del constructor para facilitar la separación de responsabilidades y mejorar la testabilidad de la clase.
        /// </summary>
        private readonly IHttpContextAccessor context = context;

        /// <summary>
        /// Logica de negocio para manejar la solicitud de obtener ensayos por fecha. Se obtiene la lista de clientes deudores que han realizado ensayos dentro del rango de fechas especificado, y se enriquecen con los detalles de sus ensayos asociados, además de convertir el nombre completo a mayúsculas para estandarizar la presentación.
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Lista de ensayos por rango de fechas</returns>
        public async Task<IEnumerable<ClienteDeudorModel>> Handle(ObtenerEnsayoFechaCom request, CancellationToken cancellationToken)
        {
            IEnumerable<ClienteDeudorModel> clientes = await _iEnsayo.ConsultarEnsayoFechas(request.FechaInicio, request.FechaFin);
            if (clientes.Any())
            {
                foreach (var item in clientes)
                {
                    item.Ensayos = await _iEnsayo.ObtenerEnsayosDetallados(item.IdEnsayo);
                    item.NombreCompleto = item.NombreCompleto?.ToUpper();
                }

                string idSesion = context.HttpContext.Request.Headers["IdSesion"].ToString();
                await _iCacheServicio.Agregar($"ConsultarEnsayoFechas_{idSesion}", clientes, TimeSpan.FromMinutes(60));
                return clientes;
            }

            return [];
        }
    }
}
