using Core.DataAccess.Clientes.Interfaz;
using Core.DataAccess.Configuracion;
using Core.Dominio.Model;
using Dapper;
using System.Data;

namespace Core.DataAccess.Clientes.Servicio
{
    public class ClienteServicio : ICliente
    {
        /// <summary>
        /// Instancia de conexión
        /// </summary>
        readonly SqlConfiguracion sqlConfiguracion;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="sqlConfiguracion">Instancia de conexión</param>
        /// <exception cref="ArgumentException">Control de excepciones</exception>
        public ClienteServicio(SqlConfiguracion sqlConfiguracion)
        {
            this.sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentException(nameof(sqlConfiguracion));
        }

        /// <summary>
        /// Consulta usuario
        /// </summary>
        /// <param name="logguin">Logguin del usuario</param>
        /// <param name="llave">Llave de cifrado</param>
        /// <returns>Usuario</returns>
        /// <exception cref="DataException">Control de errore</exception>
        public async Task<ClienteModel> ObtenerCliente(string logguin, string llave)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_al_loggin", dbType: DbType.String, value: logguin);
            parametros.Add("@i_llave", dbType: DbType.String, value: llave);
            parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            var resultado = await dbConnection.QueryFirstOrDefaultAsync<ClienteModel>("sps_usuario", parametros, commandType: CommandType.StoredProcedure);
            int respuesta = parametros.Get<int>("@ReturnValue");
            if (respuesta != 0)
            {
                throw new DataException { HResult = respuesta };
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza el numero de logguin del cliente
        /// </summary>
        /// <param name="idCliente">Id cliente</param>
        /// <exception cref="DataException">Control de errores</exception>
        public async Task ActualizarNumeroLogguin(int idCliente)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_al_id", dbType: DbType.Int32, value: idCliente);
            parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            await dbConnection.ExecuteAsync("spu_numero_logguin", parametros, commandType: CommandType.StoredProcedure);
            int respuesta = parametros.Get<int>("@ReturnValue");
            if (respuesta != 0)
            {
                throw new DataException { HResult = respuesta };
            }
        }
    }
}
