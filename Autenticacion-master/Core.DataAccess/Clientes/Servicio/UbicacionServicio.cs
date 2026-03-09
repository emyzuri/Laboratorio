using Core.DataAccess.Configuracion;
using Core.Dominio.Model.Ubicacion;
using Dapper;
using System.Data;
using Core.DataAccess.Clientes.Interfaz;

namespace Core.DataAccess.Ubicacion.Servicio
{
    public class UbicacionServicio : IUbicacion
    {
        private readonly SqlConfiguracion sqlConfiguracion;

        public UbicacionServicio(SqlConfiguracion sqlConfiguracion)
        {
            this.sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentException(nameof(sqlConfiguracion));
        }

        public async Task<IEnumerable<ProvinciaModel>> ObtenerProvincias()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            return await db.QueryAsync<ProvinciaModel>(
                "sps_obtener_provincias",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<CantonModel>> ObtenerCantones(int idProvincia)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            var p = new DynamicParameters();
            p.Add("@i_idProvincia", idProvincia, DbType.Int32);

            return await db.QueryAsync<CantonModel>(
                "sps_obtener_cantones_por_provincia",
                p,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ParroquiaModel>> ObtenerParroquias(int idCanton)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            var p = new DynamicParameters();
            p.Add("@i_idCanton", idCanton, DbType.Int32);

            return await db.QueryAsync<ParroquiaModel>(
                "sps_obtener_parroquias_por_canton",
                p,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}