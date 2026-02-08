
using Core.DataAccess.Clientes.Interfaz;
using Core.DataAccess.Configuracion;
using Core.Dominio.Clientes;
using Core.Dominio.Model;
using Core.Dominio.Request.Ensayos;
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
        public async Task RegistrarPago(int idCliente, double abono, double montoTotal, string usuario, int idPrueba)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters p = new();

            p.Add("@i_en_idCliente", dbType: DbType.Int32, value: idCliente);
            p.Add("@i_abono", dbType: DbType.Double, value: abono);
            p.Add("@i_montoTotal", dbType: DbType.Double, value: montoTotal); 
            p.Add("@i_nombreUsuario", dbType: DbType.String, value: usuario);
            p.Add("@i_idPrueba", dbType: DbType.Int32, value: idPrueba);

            await db.ExecuteAsync("sps_insertar_ensayo_con_pago", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConsultarAbonoRequest>> ObtenerAbonosPorCliente(int idCliente)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();

            return await db.QueryAsync<ConsultarAbonoRequest>(
                "sps_consultar_abonos_cliente", 
                new { i_idCliente = idCliente },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<PagoModel>> ObtenerPagosPorPrueba(int idPrueba)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            string sql = "SELECT pg_montoTotal AS MontoTotal FROM dbo.ap_pagos WHERE pg_idEnsayo = @idPrueba";
            return await db.QueryAsync<PagoModel>(sql, new { idPrueba });
        }
        public async Task<IEnumerable<ClienteDeudorModel>> ObtenerClientesDeudores()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            return await db.QueryAsync<ClienteDeudorModel>(
                "sps_obtener_clientes_deudores",
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
