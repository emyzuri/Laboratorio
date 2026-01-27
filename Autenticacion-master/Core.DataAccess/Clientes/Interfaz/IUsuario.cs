using Core.Dominio.Model;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface IUsuario
    {
        /// <summary>
        /// Consulta usuario
        /// </summary>
        /// <param name="usuario">Logguin del usuario</param>
        /// <param name="password">Llave de cifrado</param>
        /// <returns>Usuario</returns>
        /// <exception cref="DataException">Control de errore</exception>
        Task<UsuarioModel> ObtenerUsuario(string usuario, string password);
        Task<List<UsuarioModel>> ObtenerUsuarios();
    }
}
