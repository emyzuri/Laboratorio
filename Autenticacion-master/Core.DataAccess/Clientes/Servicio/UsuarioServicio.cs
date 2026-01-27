using Core.DataAccess.Clientes.Interfaz;
using Core.DataAccess.Configuracion;
using Core.Dominio.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Clientes.Servicio
{
    public class UsuarioServicio : IUsuario
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
        public UsuarioServicio(SqlConfiguracion sqlConfiguracion)
        {
            this.sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentException(nameof(sqlConfiguracion));
        }

        /// <summary>
        /// Consulta usuario
        /// </summary>
        /// <param name="usuario">Logguin del usuario</param>
        /// <param name="password">Llave de cifrado</param>
        /// <returns>Usuario</returns>
        /// <exception cref="DataException">Control de errore</exception>
        public async Task<UsuarioModel> ObtenerUsuario(string usuario, string password)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_us_login", dbType: DbType.String, value: usuario);
            parametros.Add("@i_us_password", dbType: DbType.String, value: password);
            parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            var resultado = await dbConnection.QueryFirstOrDefaultAsync<UsuarioModel>("sps_login_usuario", parametros, commandType: CommandType.StoredProcedure);
            int respuesta = parametros.Get<int>("@ReturnValue");
            if (respuesta != 0)
            {
                throw new DataException { HResult = respuesta };
            }

            return resultado;
        }

        /// <summary>
        /// Consulta usuario
        /// </summary>
        /// <returns>Usuario</returns>
        /// <exception cref="DataException">Control de errore</exception>
        public async Task<List<UsuarioModel>> ObtenerUsuarios()
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            var resultado = await dbConnection.QueryAsync<UsuarioModel>("sps_usuarios", parametros, commandType: CommandType.StoredProcedure);
            int respuesta = parametros.Get<int>("@ReturnValue");
            if (respuesta != 0)
            {
                throw new DataException { HResult = respuesta };
            }

            return resultado.ToList();
        }


    }
}
