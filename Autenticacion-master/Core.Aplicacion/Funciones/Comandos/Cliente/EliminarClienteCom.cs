using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    /// <summary>
    /// Clase transaccional para eliminar un cliente.
    /// </summary>
    /// <remarks>
    /// Contructor que inicializa el identificador del cliente a eliminar.
    /// </remarks>
    /// <param name="idCliente">Identificador del cliente</param>
    public class EliminarClienteCom(int idCliente) : IRequest<Unit>
    {
        /// <summary>
        /// Identificador del cliente a eliminar.
        /// </summary>
        public int IdCliente { get; set; } = idCliente;
    }
}