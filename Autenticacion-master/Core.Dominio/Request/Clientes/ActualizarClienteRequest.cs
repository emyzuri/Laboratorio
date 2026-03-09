namespace Core.Dominio.Request.Clientes
{
    /// <summary>
    /// Clase de solicitud para actualizar la información de un cliente. Esta clase encapsula los datos necesarios para realizar la actualización de un cliente en el sistema, incluyendo su identificador único, nombre, apellido, teléfono, dirección, ciudad, título y correo electrónico. Al utilizar esta clase como parte de una solicitud de actualización, se garantiza que se proporcionen todos los datos necesarios para llevar a cabo la operación de manera efectiva y precisa, asegurando así la integridad de la información del cliente en el sistema.
    /// </summary>
    public class ActualizarClienteRequest
    {
        /// <summary>
        /// Identificador único del cliente a actualizar. Este campo es esencial para localizar el registro específico del cliente en la base de datos y aplicar las modificaciones correspondientes. Sin este identificador, no sería posible determinar qué cliente se desea actualizar, lo que podría llevar a errores o inconsistencias en los datos.
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Nombre del cliente a actualizar. Este campo representa el nombre del cliente y es uno de los datos principales que se pueden modificar en el proceso de actualización. Es importante asegurarse de que este campo se complete correctamente, ya que el nombre es una parte fundamental de la identidad del cliente y puede ser utilizado para fines de identificación, comunicación y registro en la base de datos.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido del cliente a actualizar. Este campo representa el apellido del cliente y es otro dato importante que se puede modificar durante el proceso de actualización. Al igual que el nombre, el apellido es una parte esencial de la identidad del cliente y puede ser utilizado para fines de identificación, comunicación y registro en la base de datos. Es crucial asegurarse de que este campo se complete correctamente para mantener la integridad de los datos del cliente y facilitar su gestión en el sistema.
        /// </summary>
        public string Apellido { get; set; }

        /// <summary>
        /// Telefono del cliente a actualizar. Este campo representa el número de teléfono del cliente y es un dato importante para la comunicación y contacto con el cliente. Al actualizar este campo, es fundamental asegurarse de que el número de teléfono se ingrese correctamente, ya que un número incorrecto podría dificultar la comunicación con el cliente y afectar la calidad del servicio. Además, el número de teléfono puede ser utilizado para fines de identificación y registro en la base de datos, por lo que su precisión es crucial para mantener la integridad de los datos del cliente en el sistema.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Direccion del cliente a actualizar. Este campo representa la dirección del cliente y es un dato importante para la gestión de la información del cliente en el sistema. Al actualizar este campo, es esencial asegurarse de que la dirección se ingrese correctamente, ya que una dirección incorrecta podría dificultar la entrega de productos o servicios al cliente, así como afectar la calidad del servicio. Además, la dirección puede ser utilizada para fines de identificación y registro en la base de datos, por lo que su precisión es crucial para mantener la integridad de los datos del cliente en el sistema.
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Ciudad del cliente a actualizar. Este campo representa la ciudad de residencia del cliente y es un dato importante para la gestión de la información del cliente en el sistema. Al actualizar este campo, es fundamental asegurarse de que la ciudad se ingrese correctamente, ya que una ciudad incorrecta podría dificultar la entrega de productos o servicios al cliente, así como afectar la calidad del servicio. Además, la ciudad puede ser utilizada para fines de identificación y registro en la base de datos, por lo que su precisión es crucial para mantener la integridad de los datos del cliente en el sistema.
        /// </summary>
        public string Ciudad { get; set; }

        /// <summary>
        /// Titulo del cliente a actualizar. Este campo representa el título o cargo del cliente y es un dato importante para la gestión de la información del cliente en el sistema. Al actualizar este campo, es esencial asegurarse de que el título se ingrese correctamente, ya que un título incorrecto podría afectar la percepción del cliente y la calidad del servicio. Además, el título puede ser utilizado para fines de identificación y registro en la base de datos, por lo que su precisión es crucial para mantener la integridad de los datos del cliente en el sistema.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Correo del cliente a actualizar. Este campo representa la dirección de correo electrónico del cliente y es un dato importante para la comunicación y contacto con el cliente. Al actualizar este campo, es fundamental asegurarse de que la dirección de correo electrónico se ingrese correctamente, ya que una dirección incorrecta podría dificultar la comunicación con el cliente y afectar la calidad del servicio. Además, el correo electrónico puede ser utilizado para fines de identificación y registro en la base de datos, por lo que su precisión es crucial para mantener la integridad de los datos del cliente en el sistema.
        /// </summary>
        public string Correo { get; set; }
    }
}
