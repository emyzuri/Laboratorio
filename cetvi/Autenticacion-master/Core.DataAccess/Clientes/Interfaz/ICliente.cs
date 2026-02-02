using Core.Dominio.Model;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface ICliente
    {
        /// <summary>
        /// Consulta usuario
        /// </summary>
        /// <param name="logguin">Logguin del usuario</param>
        /// <param name="llave">Llave de cifrado</param>
        /// <returns>Usuario</returns>
        /// <exception cref="DataException">Control de errore</exception>
        Task<ClienteModel> ObtenerCliente(string logguin, string llave);

        /// <summary>
        /// Actualiza el numero de logguin del cliente
        /// </summary>
        /// <param name="idCliente">Id cliente</param>
        /// <exception cref="DataException">Control de errores</exception>
        Task ActualizarNumeroLogguin(int idCliente);
    }
}
