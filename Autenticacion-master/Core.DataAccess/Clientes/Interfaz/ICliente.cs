using Core.Dominio.Clientes;
using Core.Dominio.Model;

namespace Core.DataAccess.Clientes.Interfaz
{
    public interface ICliente
    {
        Task<ClienteModel> ObtenerCliente(int idCliente);

        /// <summary>
        /// Elimina un cliente de la base de datos, permitiendo gestionar la información de los clientes de manera eficiente y mantener la integridad de los datos en el sistema. Este método recibe el ID del cliente a eliminar, y realiza la eliminación a través de un procedimiento almacenado llamado "spd_clientes", que se encarga de manejar la lógica de eliminación y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente</param>
        /// <returns>Cliente eliminado</returns>
        Task DesactivarCliente(int idCliente);

        /// <summary>
        /// Actualiza la información de un cliente existente en la base de datos, permitiendo mantener los datos actualizados y precisos. Este método recibe un objeto ClienteModel con los datos del cliente a actualizar, incluyendo su ID para identificar el registro correspondiente en la base de datos. La actualización se realiza a través de un procedimiento almacenado llamado "spu_clientes", que se encarga de manejar la lógica de actualización y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns></returns>
        Task ActualizarCliente(ClienteModel cliente);

        /// <summary>
        /// Consulta lista de clientes registrados en el sistema, facilitando la gestión de información y la toma de decisiones basada en los datos de los clientes.
        /// </summary>
        /// <returns>Lista de clientes</returns>
        Task<IEnumerable<ClienteModel>> ConsultarClientes();

        /// <summary>
        /// Inserta cliente en la base de datos, permitiendo agregar nuevos clientes al sistema y gestionar su información de manera eficiente. Este método recibe un objeto ClienteModel con los datos del cliente a insertar, y devuelve un objeto CrearClienteModel con la información del cliente creado, incluyendo su ID generado por la base de datos. La inserción se realiza a través de un procedimiento almacenado llamado "spi_clientes", que se encarga de manejar la lógica de inserción y garantizar la integridad de los datos en la base de datos. Al utilizar Dapper para ejecutar el procedimiento almacenado, se mejora el rendimiento y se simplifica el acceso a los datos, facilitando la gestión de clientes en el sistema.
        /// </summary>
        /// <param name="cliente">Cliente a almacenar</param>
        /// <returns>Cliente</returns>
        Task<ClienteModel> InsertarCliente(ClienteModel cliente);
        Task<CrearClienteModel> ConsultarCliente(ConsultarClienteModel cliente);
        Task ActivarCliente(string cedula);
    }
}