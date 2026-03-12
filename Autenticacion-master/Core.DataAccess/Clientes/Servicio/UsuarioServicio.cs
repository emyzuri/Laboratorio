using Core.DataAccess.Clientes.Interfaz;
using Core.DataAccess.Configuracion;
using Core.Dominio.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        public async Task<List<UsuarioModel>> ObtenerUsuariosLista()
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            var resultado = await dbConnection.QueryAsync<UsuarioModel>(
                "sps_consultar_usuarios_lista",
                commandType: CommandType.StoredProcedure
            );

            return resultado.ToList();
        }
        /// <summary>
        /// Registra un nuevo usuario y su rol asociado en la base de datos
        /// </summary>
        /// <param name="usuario">Modelo con la información del usuario y el ID del rol</param>
        /// <returns>True si el registro fue exitoso, False en caso de error</returns>
        public async Task<bool> RegistrarUsuario(UsuarioModel usuario, List<int> roles)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            dbConnection.Open();

            using var transaction = dbConnection.BeginTransaction();

            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@i_nombre", usuario.Nombre);
                parametros.Add("@i_apellido", usuario.Apellido);
                parametros.Add("@i_usuario", usuario.Usuario);
                parametros.Add("@i_password", usuario.Password);
                parametros.Add("@i_telefono", usuario.Telefono);
                parametros.Add("@i_cedula", usuario.Cedula);
                parametros.Add("@i_roles", string.Join(",", roles));

                parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync(
                    "spi_registrar_usuario_con_roles",
                    parametros,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );

                int respuesta = parametros.Get<int>("@ReturnValue");

                if (respuesta != 0)
                {
                    transaction.Rollback();
                    return false;
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        /// <summary>
        /// Actualiza los roles de un usuario existente
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <param name="roles">Lista de IDs de roles</param>
        /// <returns>True si fue exitoso</returns>
        public async Task<bool> ActualizarRolesUsuario(int idUsuario, List<int> roles)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();

            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@i_id_usuario", idUsuario); 

                string rolesString = string.Join(",", roles);
                parametros.Add("@i_roles", rolesString);

                parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await dbConnection.ExecuteAsync(
                    "spu_actualizar_roles_usuario",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );

                int respuesta = parametros.Get<int>("@ReturnValue");
                return respuesta == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<RolModel>> ObtenerRoles()
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            string sql = "SELECT rl_id as IdRol, rl_nombre as NombreRol, rl_descripcion as Descripcion FROM dbo.ap_roles";
            var resultado = await dbConnection.QueryAsync<RolModel>(sql);
            return resultado.ToList();
        }
        /// <summary>
        /// Elimina de forma lógica o física un usuario de la base de datos
        /// </summary>
        /// <param name="idUsuario">Identificador único del usuario</param>
        /// <returns>True si la eliminación fue exitosa</returns>
        public async Task<bool> EliminarUsuario(int idUsuario)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            try
            {
                DynamicParameters parametros = new();
                parametros.Add("@i_id_usuario", idUsuario, dbType: DbType.Int32);
                parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await dbConnection.ExecuteAsync("spd_eliminar_usuario", parametros, commandType: CommandType.StoredProcedure);

                int respuesta = parametros.Get<int>("@ReturnValue");
                return respuesta == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> ActualizarUsuario(UsuarioModel usuario)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@i_id_usuario", usuario.IdUsuario); 
                parametros.Add("@i_nombre", usuario.Nombre);
                parametros.Add("@i_apellido", usuario.Apellido);
                parametros.Add("@i_usuario", usuario.Usuario);
                parametros.Add("@i_telefono", usuario.Telefono);
                parametros.Add("@i_cedula", usuario.Cedula);
                parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await dbConnection.ExecuteAsync("spu_actualizar_usuario", parametros, commandType: CommandType.StoredProcedure);
                return parametros.Get<int>("@ReturnValue") == 0;
            }
            catch { return false; }
        }
    }
}
