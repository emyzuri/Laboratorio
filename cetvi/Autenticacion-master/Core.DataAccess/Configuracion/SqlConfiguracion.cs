using Microsoft.Data.SqlClient;
using System.Data;

namespace Core.DataAccess.Configuracion
{
    /// <summary>
    /// Configuración de conexión a Sql
    /// </summary>
    public class SqlConfiguracion
    {
        /// <summary>
        /// Cadena de conexión a Sql
        /// </summary>
        readonly string cadenaConexion;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public SqlConfiguracion()
        {
            cadenaConexion = "Server=EMY;Database=ap_aplicacion; Integrated Security=True;TrustServerCertificate=True;";
            //cadenaConexion = "Server=localhost;Database=ap_aplicacion;Trusted;TrustServerCertificate=True;";
            //cadenaConexion = Environment.GetEnvironmentVariable("BDD_CONEXION_AUTENTICACION", EnvironmentVariableTarget.Machine);
        }

        /// <summary>
        /// Crea la conexión
        /// </summary>
        /// <returns>Conexión a Sql</returns>
        public IDbConnection CrearConexion() => new SqlConnection(cadenaConexion);
    }
}
