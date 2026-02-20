
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

            await db.ExecuteAsync("spi_ensayo", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ConsultarUltimoIdPrueba()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters p = new();

            return await db.ExecuteScalarAsync<int>("sps_id_prueba", p, commandType: CommandType.StoredProcedure);
        }
        public async Task RegistrarPago(int idCliente, decimal abono, decimal montoTotal, string usuario, int idPrueba, DateTime fechaEntrega, string descripcion)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters p = new();

            p.Add("@i_en_idCliente", dbType: DbType.Int32, value: idCliente);
            p.Add("@i_abono", dbType: DbType.Decimal, value: abono);
            p.Add("@i_montoTotal", dbType: DbType.Decimal, value: montoTotal); 
            p.Add("@i_nombreUsuario", dbType: DbType.String, value: usuario);
            p.Add("@i_idPrueba", dbType: DbType.Int32, value: idPrueba);
            p.Add("@i_fecha_entrega", dbType: DbType.DateTime, value: fechaEntrega);
            p.Add("@i_descripcion", dbType: DbType.String, value: descripcion);

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
        public async Task<IEnumerable<CatalogoEnsayoModel>> ObtenerCatalogoEnsayo()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            IEnumerable<CatalogoEnsayoModel> resultado = await db.QueryAsync<CatalogoEnsayoModel>("sps_obtener_catalogo_ensayo", commandType: CommandType.StoredProcedure);

            return resultado;
        }
        public async Task<bool> RegistrarNuevoAbono(int idEnsayo, decimal monto, string usuario)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            var parametros = new DynamicParameters();
            parametros.Add("@i_id_prueba", dbType: DbType.Int32, value: idEnsayo);
            parametros.Add("@i_abono", dbType: DbType.Decimal, value: monto);
            parametros.Add("@i_usuario", dbType: DbType.String, value: usuario);
            int filas = await db.ExecuteAsync(
                "spi_insertar_abono",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return true;
        }
        public async Task<IEnumerable<EnsayoDetalladoModel>> ObtenerEnsayosDetallados(int idPrueba)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            var parametros = new DynamicParameters();
            parametros.Add("@i_id_prueba", dbType: DbType.Int32, value: idPrueba);
            return await db.QueryAsync<EnsayoDetalladoModel>(
                "sps_obtener_ensayos_detallados",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
