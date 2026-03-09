using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    /// <summary>
    /// Logica de negocio para consultar clientes, se valida que la sesión no haya caducado antes de realizar la consulta.
    /// </summary>
    /// <param name="iCliente">Servicio de clientes</param>
    /// <param name="cacheServicio">Servicio de memoria cache</param>
    /// <param name="context">Http contexto</param>
    public class ConsultarClienteHandler(ICliente iCliente, ICacheServicio cacheServicio) : IRequestHandler<ConsultarClienteCom, IEnumerable<ClienteModel>>
    {
        /// <summary>
        /// Servico de clientes, utilizado para obtener información de clientes desde la base de datos.
        /// </summary>
        private readonly ICliente iCliente = iCliente ?? throw new ArgumentException(nameof(iCliente));

        /// <summary>
        /// Servicio de memoria cache, utilizado para almacenar temporalmente los resultados de las consultas, mejorando el rendimiento y reduciendo la carga en la base de datos al evitar consultas repetitivas para la misma información dentro de un período de tiempo determinado. Este servicio se inyecta a través del constructor para facilitar la separación de responsabilidades y mejorar la testabilidad de la clase.
        /// </summary>
        readonly ICacheServicio cacheServicio = cacheServicio ?? throw new ArgumentException(nameof(cacheServicio));

        /// <summary>
        /// Logica de negocio para manejar la solicitud de consultar clientes. Se valida que la sesión no haya caducado antes de realizar la consulta, si la sesión es válida se obtiene la lista de clientes desde el servicio de clientes, y se almacena en cache para futuras consultas dentro del mismo período de tiempo. Si la sesión ha caducado, se devuelve la información almacenada en cache para evitar realizar una consulta a la base de datos y mejorar el rendimiento.
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Lista de clientes</returns>
        public async Task<IEnumerable<ClienteModel>> Handle(ConsultarClienteCom request, CancellationToken cancellationToken)
        {
            if (await cacheServicio.Existe("Clientes_"))
            {
                return await cacheServicio.Obtener<IEnumerable<ClienteModel>>("Clientes_");
            }
            else
            {
                IEnumerable<ClienteModel> clientes = await iCliente.ConsultarClientes();
                await cacheServicio.Agregar($"Clientes_", clientes, TimeSpan.FromMinutes(480));
                return await iCliente.ConsultarClientes();
            }
        }
    }
}