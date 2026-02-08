using Core.DataAccess.Configuracion;
using Core.Dominio.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Menu.Interfaz;

namespace Core.DataAccess.Menu.Servicio
{
    public class MenuServicio : IMenu
    {
        readonly SqlConfiguracion sqlConfiguracion;

        public MenuServicio(SqlConfiguracion sqlConfiguracion)
        {
            this.sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentException(nameof(sqlConfiguracion));
        }

        public async Task<IEnumerable<MenuModel>> ObtenerMenu()
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            IEnumerable<MenuModel> resultado = await dbConnection.QueryAsync<MenuModel>("sps_menus", parametros, commandType: CommandType.StoredProcedure);

            int respuesta = parametros.Get<int>("@ReturnValue");
            if (respuesta != 0)
            {
                throw new DataException { HResult = respuesta };
            }

            return resultado;
        }

        public async Task<IEnumerable<MenuModel>> ObtenerMenus()
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            IEnumerable<MenuModel> resultado = await dbConnection.QueryAsync<MenuModel>("sps_menus", parametros, commandType: CommandType.StoredProcedure);

            int respuesta = parametros.Get<int>("@ReturnValue");
            if (respuesta != 0)
            {
                throw new DataException { HResult = respuesta };
            }

            return resultado;
        }
    }
}