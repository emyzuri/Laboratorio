using Core.DataAccess.Clientes.Interfaz;
using Core.DataAccess.Configuracion;
using Core.Dominio.Clientes;
using Core.Dominio.Model;
using Core.Dominio.Request.Ensayos;
using Dapper;
using System.Data;

namespace Core.DataAccess.Clientes.Servicio
{
    public class EnsayoServicio(SqlConfiguracion sqlConfiguracion) : IEnsayo
    {
        /// <summary>
        /// Constructor que inyecta la configuración de SQL para establecer la conexión a la base de datos. Esta configuración es esencial para ejecutar las consultas y procedimientos almacenados relacionados con los ensayos, pagos y clientes deudores.
        /// </summary>
        private readonly SqlConfiguracion sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentException(nameof(sqlConfiguracion));

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
            return await db.ExecuteScalarAsync<int>("sps_id_prueba", null, commandType: CommandType.StoredProcedure);
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
            return await db.QueryAsync<ConsultarAbonoRequest>("sps_consultar_abonos_cliente", new { i_idCliente = idCliente }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PagoModel>> ObtenerPagosPorPrueba(int idPrueba)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            string sql = "SELECT pg_montoTotal AS MontoTotal FROM dbo.ap_pagos WHERE pg_idEnsayo = @idPrueba";
            return await db.QueryAsync<PagoModel>(sql, new { idPrueba });
        }

        /// <summary>
        /// Consulta de clientes deudores, es decir, aquellos que tienen pagos pendientes o deudas relacionadas con los ensayos realizados. Esta consulta puede incluir información como el nombre del cliente, el monto adeudado, la fecha del último pago y detalles de los ensayos asociados a la deuda.
        /// </summary>
        /// <returns>Lista de clientes deudores</returns>
        public async Task<IEnumerable<ClienteDeudorModel>> ObtenerClientesDeudores()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            return await db.QueryAsync<ClienteDeudorModel>("sps_obtener_clientes_deudores", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// COnsulta de ensayos por rango de fechas. Permite filtrar los ensayos realizados dentro de un período determinado, facilitando la generación de reportes y análisis de datos históricos. Esta consulta puede incluir información como el nombre del cliente, el monto adeudado, la fecha del último pago y detalles de los ensayos asociados a la deuda, además de convertir el nombre completo a mayúsculas para estandarizar la presentación.
        /// </summary>
        /// <param name="fechaInicio">Fecha inicio del filtro</param>
        /// <param name="fechaFin">Fecha fin del filtro</param>
        /// <returns>LIsta de ensayos</returns>
        public async Task<IEnumerable<ClienteDeudorModel>> ConsultarEnsayoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();

            DynamicParameters parametros = new();
            parametros.Add("@w_fecha_inicio", dbType: DbType.DateTime, value: fechaInicio);
            parametros.Add("@w_fecha_fin", dbType: DbType.DateTime, value: fechaFin);

            return await db.QueryAsync<ClienteDeudorModel>(
                "sps_ensayo_fechas",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ClienteDeudorModel>> ConsultarDeudaEnsayo(int idPrueba)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();

            DynamicParameters parametros = new();
            parametros.Add("@w_id_prueba", dbType: DbType.Int32, value: idPrueba);

            return await db.QueryAsync<ClienteDeudorModel>(
                "sps_ensayo_id",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<CatalogoEnsayoModel>> ObtenerCatalogoEnsayo(int? idPadre = null)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();

            var p = new { i_idPadre = (idPadre == 0 ? null : idPadre) };

            return await db.QueryAsync<CatalogoEnsayoModel>(
                "sps_obtener_catalogo_por_padre",
                p,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<CatalogoEnsayoModel>> ObtenerCatalogoJerarquico()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            string sql = "SELECT ct_id AS Id, ct_nombre AS Nombre, ct_descripcion AS Descripcion, ct_id_padre AS IdPadre FROM ap_catalogo WHERE ct_estado = 1";
            var todoElCatalogo = (await db.QueryAsync<CatalogoEnsayoModel>(sql)).ToList();

            var padres = todoElCatalogo.Where(x => x.IdPadre == null || x.IdPadre == 0).ToList();

            foreach (var padre in padres)
            {
                padre.Hijos = todoElCatalogo.Where(x => x.IdPadre == padre.Id).ToList();
            }

            return padres;
        }
        public async Task<bool> RegistrarNuevoAbono(int idEnsayo, decimal monto, string usuario)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_id_prueba", dbType: DbType.Int32, value: idEnsayo);
            parametros.Add("@i_abono", dbType: DbType.Decimal, value: monto);
            parametros.Add("@i_usuario", dbType: DbType.String, value: usuario);
            await db.ExecuteAsync("spi_insertar_abono", parametros, commandType: CommandType.StoredProcedure);
            return true;
        }

        /// <summary>
        /// Consulta detallada de los ensayos realizados para una prueba específica, incluyendo información del catálogo y detalles relevantes.
        /// </summary>
        /// <param name="idPrueba">Identificador del ensayo</param>
        /// <returns>Lista de ensayos realizados</returns>
        public async Task<IEnumerable<EnsayoDetalladoModel>> ObtenerEnsayosDetallados(int idPrueba)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_id_prueba", dbType: DbType.Int32, value: idPrueba);
            return await db.QueryAsync<EnsayoDetalladoModel>("sps_obtener_ensayos_detallados", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteEnsayoModel>> ObtenerReportePorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();

            DynamicParameters parametros = new();
            parametros.Add("@i_fechaInicio", dbType: DbType.DateTime, value: fechaInicio);
            parametros.Add("@i_fechaFin", dbType: DbType.DateTime, value: fechaFin);

            return await db.QueryAsync<ReporteEnsayoModel>(
                "sps_reporte_ensayos_por_fecha",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// Consulta de ensayos por cédula del cliente. Permite obtener una lista de clientes deudores que han realizado ensayos dentro del rango de fechas especificado, y se enriquecen con los detalles de sus ensayos asociados, además de convertir el nombre completo a mayúsculas para estandarizar la presentación.
        /// </summary>
        /// <param name="cedula">Identificacion del cliente</param>
        /// <param name="fechaInicio">Fecha inicio del filtro</param>
        /// <param name="fechaFin">Fecha fin del filtro</param>
        /// <returns>Lista de ensayos</returns>
        public async Task<IEnumerable<ClienteDeudorModel>> ConsultarPorCedula(string cedula, DateTime fechaInicio, DateTime fechaFin)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters paremetros = new();
            paremetros.Add("@w_cedula", dbType: DbType.String, value: cedula);
            paremetros.Add("@w_fecha_inicio", dbType: DbType.DateTime, value: fechaInicio);
            paremetros.Add("@w_fecha_fin", dbType: DbType.DateTime, value: fechaFin);

            return await db.QueryAsync<ClienteDeudorModel>(
                "sps_consultar_ensayos_cliente",
                paremetros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}