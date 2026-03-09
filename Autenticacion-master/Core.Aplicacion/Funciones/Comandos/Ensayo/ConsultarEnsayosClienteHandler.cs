using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using Polly;

namespace Core.Aplicacion.Funciones.Comandos.Ensayo
{
    /// <summary>
    /// Logica de negocio para obtener ensayos asociados a un cliente específico. Esta clase maneja la solicitud de consulta de ensayos por cédula del cliente, y devuelve una lista de modelos que representan a los clientes deudores junto con los detalles de sus ensayos asociados. Además, se estandariza la presentación del nombre completo del cliente convirtiéndolo a mayúsculas.
    /// </summary>
    public class ConsultarEnsayosClienteHandler(IEnsayo iEnsayo, ICacheServicio cacheServicio, IHttpContextAccessor context) : IRequestHandler<ConsultarEnsayosClienteCom, IEnumerable<ClienteDeudorModel>>
    {
        /// <summary>
        /// Servicio de acceso a datos para ensayos, utilizado para obtener información de clientes deudores y sus ensayos asociados. Este servicio es esencial para realizar las consultas necesarias para obtener los datos requeridos por la solicitud, y se inyecta a través del constructor para facilitar la separación de responsabilidades y mejorar la testabilidad de la clase.
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
        /// Logica de negocio para manejar la solicitud de obtener ensayos por cédula del cliente. Se obtiene la lista de clientes deudores que han realizado ensayos dentro del rango de fechas especificado, y se enriquecen con los detalles de sus ensayos asociados, además de convertir el nombre completo a mayúsculas para estandarizar la presentación.
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Lista de ensayos</returns>
        public async Task<IEnumerable<ClienteDeudorModel>> Handle(ConsultarEnsayosClienteCom request, CancellationToken cancellationToken)
        {
            IEnumerable<ClienteDeudorModel> resultados = await _iEnsayo.ConsultarPorCedula(request.Cedula, request.FechaInicio, request.FechaFin);

            if (resultados.Any())
            {
                foreach (var item in resultados)
                {
                    item.Ensayos = await _iEnsayo.ObtenerEnsayosDetallados(item.IdEnsayo);
                    item.NombreCompleto = item.NombreCompleto?.ToUpper();
                }

                string idSesion = context.HttpContext.Request.Headers["IdSesion"].ToString();
                await _iCacheServicio.Agregar($"ConsultarPorCedula_{idSesion}", resultados, TimeSpan.FromMinutes(60));
                return resultados;

            }

            return [];
        }
    }
}
