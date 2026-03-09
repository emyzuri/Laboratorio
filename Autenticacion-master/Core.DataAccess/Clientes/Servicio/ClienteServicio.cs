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

        /// <summary>
        /// Elimina un cliente de la base de datos, permitiendo gestionar la información de los clientes de manera eficiente y mantener la integridad de los datos en el sistema. Este método recibe el ID del cliente a eliminar, y realiza la eliminación a través de un procedimiento almacenado llamado "spd_clientes", que se encarga de manejar la lógica de eliminación y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente</param>
        /// <returns>Cliente eliminado</returns>
        public async Task DesactivarCliente(int idCliente)
        {
            using IDbConnection dbConnection = sqlConfiguracion.CrearConexion();
            DynamicParameters parametros = new();
            parametros.Add("@i_cl_id", idCliente);
            parametros.Add("@i_cl_estado", 0);

            await dbConnection.ExecuteScalarAsync<int>("spu_clientes_estado", parametros, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Actualiza la información de un cliente existente en la base de datos, permitiendo mantener los datos actualizados y precisos. Este método recibe un objeto ClienteModel con los datos del cliente a actualizar, incluyendo su ID para identificar el registro correspondiente en la base de datos. La actualización se realiza a través de un procedimiento almacenado llamado "spu_clientes", que se encarga de manejar la lógica de actualización y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="cliente">Cliente</param>
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
            parametros.Add("@i_cl_correo", dbType: DbType.String, value: cliente.Correo);

            await dbConnection.ExecuteAsync("spu_clientes", parametros, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Consulta lista de clientes registrados en el sistema, facilitando la gestión de información y la toma de decisiones basada en los datos de los clientes.
        /// </summary>
        /// <returns>Lista de clientes</returns>
        public async Task<IEnumerable<ClienteModel>> ConsultarClientes()
        {
            using IDbConnection db = sqlConfiguracion.CrearConexion();
            IEnumerable<ClienteModel> resultado = await db.QueryAsync<ClienteModel>("sps_clientes", commandType: CommandType.StoredProcedure);

            return resultado;
        }

        /// <summary>
        /// Inserta cliente en la base de datos, permitiendo agregar nuevos clientes al sistema y gestionar su información de manera eficiente. Este método recibe un objeto ClienteModel con los datos del cliente a insertar, y devuelve un objeto CrearClienteModel con la información del cliente creado, incluyendo su ID generado por la base de datos. La inserción se realiza a través de un procedimiento almacenado llamado "spi_clientes", que se encarga de manejar la lógica de inserción y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="cliente">Cliente a almacenar</param>
        /// <returns>Cliente</returns>
        public async Task<ClienteModel> InsertarCliente(ClienteModel cliente)
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
            p.Add("@i_cl_correo", cliente.Correo);

            return await db.QueryFirstOrDefaultAsync<ClienteModel>("spi_clientes", p, commandType: CommandType.StoredProcedure);
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