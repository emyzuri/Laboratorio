using Core.Dominio.Comunes;

namespace Core.Dominio.Model
{
    public class ClienteModel : EntidadAuditoriaBase
    {
        private int edad;

        /// <summary>
        /// Identificador del cliewnte
        /// </summary>
        public int IdCliente { get; set; }

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
        /// Identificacion del cliente
        /// </summary>
        public string Identificacion { get; set; }

        /// <summary>
        /// Calve del cliente
        /// </summary>
        public string Clave { get; set; }

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

        /// <summary>
        /// Estado loggin
        /// </summary>
        public string EstadoLoggin { get; set; } 

        /// <summary>
        /// Intentos de logguin
        /// </summary>
        public int IntentoLoggin { get; set; } 

        /// <summary>
        /// Edad del cliente
        /// </summary>
        public int Edad
        {
            get
            {
                if (this.edad <= 0)
                {
                    this.edad = new DateTime(DateTime.Now.Subtract(this.FechaNacimiento).Ticks).Year - 1;
                }
                return this.edad;
            }
        }
    }
}
