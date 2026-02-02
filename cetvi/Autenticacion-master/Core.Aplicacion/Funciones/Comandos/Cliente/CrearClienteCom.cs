using Core.Aplicacion.RespuestaUtilitario;
using MediatR;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    public class CrearClienteCom : IRequest<Respuesta<int>>
    {
        /// <summary>
        /// Primer nombre del cliente
        /// </summary>
        public string PrimerNombre { get; set; }

        /// <summary>
        /// Segundo nombre del cliente
        /// </summary>
        public string SegundoNombre { get; set; }

        /// <summary>
        /// Primer apellido del cliente
        /// </summary>
        public string PrimerApellido { get; set; }

        /// <summary>
        /// Segundo apellido del cliente
        /// </summary>
        public string SegundoApellido { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Teléfono del cliente
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Correo del cliente
        /// </summary>
        public string Correo { get; set; }
    }
}
