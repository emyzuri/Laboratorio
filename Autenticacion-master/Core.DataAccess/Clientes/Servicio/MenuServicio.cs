using Core.DataAccess.Configuracion;
using Core.Dominio.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Core.DataAccess.Menu.Interfaz;

namespace Core.DataAccess.Menu.Servicio
{
    public class MenuServicio : IMenu
    {
        readonly SqlConfiguracion sqlConfiguracion;

        public MenuServicio(SqlConfiguracion sqlConfiguracion)
        {
            this.sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentNullException(nameof(sqlConfiguracion));
        }
        public async Task<IEnumerable<MenuModel>> ObtenerMenu()
        {
            return await ObtenerMenus();
        }

        public async Task<IEnumerable<MenuModel>> ObtenerMenus()
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            return await dbConnection.QueryAsync<MenuModel>(
                "sps_menus",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MenuModel>> ObtenerMenusPorRol(int idRol)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@w_ur_id_usuario", idRol);

            return await dbConnection.QueryAsync<MenuModel>(
                "sps_obtener_menu_por_rol",
                parametros,
                commandType: CommandType.StoredProcedure);
        }
    }
}