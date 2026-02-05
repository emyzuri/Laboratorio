
using Core.DataAccess.Clientes.Interfaz;
using Core.DataAccess.Configuracion;
using Core.Dominio.Clientes;
using Core.Dominio.Model;
using Dapper;
using System.Data;

namespace Core.DataAccess.Clientes.Servicio
{
    public class EnsayoServicio : IEnsayo
    {
        private readonly SqlConfiguracion sqlConfiguracion;

        public EnsayoServicio(SqlConfiguracion sqlConfiguracion)
        {
            this.sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentException(nameof(sqlConfiguracion));
        }

        public async Task InsertarEnsayo(EnsayoModel ensayo, int idCliente, int idPrueba, string usuario)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters p = new();

            p.Add("@i_en_idCliente", dbType: DbType.Int32, value: idCliente);
            p.Add("@i_en_idCatalogo", dbType: DbType.Int32, value: ensayo.IdCatalogo);
            p.Add("@i_en_idPrueba", dbType: DbType.Int32, value: idPrueba);
            p.Add("@i_en_nombreUsuario", dbType: DbType.String, value: usuario);
            p.Add("@i_en_monto", dbType: DbType.Double, value: ensayo.Monto);

            await db.ExecuteAsync("spi_ensayo", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ConsultarUltimoIdPrueba()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters p = new();

            return await db.ExecuteScalarAsync<int>("sps_id_prueba", p, commandType: CommandType.StoredProcedure);
        }

    }
}
