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
    public class RegistrarLogServicio : IRegistrarLog
    {
        readonly SqlConfiguracion sqlConfiguracion;

        public RegistrarLogServicio(SqlConfiguracion sqlConfiguracion)
        {
            this.sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentException(nameof(sqlConfiguracion));
        }

        public async Task RegistrarLog(string controlador, string response, string usuario)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@w_log_controlador", dbType: DbType.String, value: controlador);
            parametros.Add("@w_log_response", dbType: DbType.String, value: response);
            parametros.Add("@w_log_usuario", dbType: DbType.String, value: usuario);
            parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            await dbConnection.QueryFirstOrDefaultAsync<RegistroLogModel>("spi_log", parametros, commandType: CommandType.StoredProcedure);
        }
    }
}
