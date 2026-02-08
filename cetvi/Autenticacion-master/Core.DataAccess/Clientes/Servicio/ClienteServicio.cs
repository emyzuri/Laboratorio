using Core.DataAccess.Clientes.Interfaz;
using Core.DataAccess.Configuracion;
using Core.Dominio.Clientes;
using Core.Dominio.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DataAccess.Clientes.Servicio
{
    public class ClienteServicio : ICliente
    {
        private readonly SqlConfiguracion sqlConfiguracion;

        public ClienteServicio(SqlConfiguracion sqlConfiguracion)
        {
            this.sqlConfiguracion = sqlConfiguracion ?? throw new ArgumentException(nameof(sqlConfiguracion));
        }

        public async Task<ClienteModel> ObtenerCliente(int idCliente)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_cl_id", dbType: DbType.Int32, value: idCliente);

            return await dbConnection.QueryFirstOrDefaultAsync<ClienteModel>("sps_clientes", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task DesactivarCliente(int idCliente)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_cl_id", idCliente);
            parametros.Add("@i_cl_estado", 0);

            await dbConnection.ExecuteScalarAsync<int>("spu_clientes_estado", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task ActualizarCliente(ClienteModel cliente)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_cl_id", dbType: DbType.Int32, value: cliente.IdCliente);
            parametros.Add("@i_cl_nombre", dbType: DbType.String, value: cliente.Nombre);
            parametros.Add("@i_cl_apellido", dbType: DbType.String, value: cliente.Apellido);
            parametros.Add("@i_cl_telefono", dbType: DbType.String, value: cliente.Telefono);
            parametros.Add("@i_cl_direccion", dbType: DbType.String, value: cliente.Direccion);
            parametros.Add("@i_cl_ciudad", dbType: DbType.String, value: cliente.Ciudad);
            parametros.Add("@i_cl_titulo", dbType: DbType.String, value: cliente.Titulo);

            await dbConnection.ExecuteAsync("spu_clientes", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ClienteModel>> ConsultarClientes()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            IEnumerable<ClienteModel> resultado = await db.QueryAsync<ClienteModel>("sps_clientes", commandType: CommandType.StoredProcedure);

            return resultado;
        }
        public async Task<CrearClienteModel> InsertarCliente(ClienteModel cliente)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters p = new();

            p.Add("@i_cl_cedula", cliente.Cedula);
            p.Add("@i_cl_nombre", cliente.Nombre);
            p.Add("@i_cl_apellido", cliente.Apellido);
            p.Add("@i_cl_telefono", cliente.Telefono);
            p.Add("@i_cl_direccion", cliente.Direccion);
            p.Add("@i_cl_ciudad", cliente.Ciudad);
            p.Add("@i_cl_titulo", cliente.Titulo);

            var clienteResp = await db.QueryFirstOrDefaultAsync<CrearClienteModel>("spi_clientes", p, commandType: CommandType.StoredProcedure);
            return clienteResp;
        }

        public async Task<CrearClienteModel> ConsultarCliente(ConsultarClienteModel cliente)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters p = new();

            p.Add("@i_cl_cedula", cliente.Cedula);

            var clienteResp = await db.QueryFirstOrDefaultAsync<CrearClienteModel>("sps_cliente_cedula", p, commandType: CommandType.StoredProcedure);
            return clienteResp;
        }

        public async Task ActivarCliente(string cedula)
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            DynamicParameters p = new();

            p.Add("@i_cl_cedula", cedula);

            await db.QueryFirstOrDefaultAsync<CrearClienteModel>("spu_cliente_activar", p, commandType: CommandType.StoredProcedure);
        }

    }
}