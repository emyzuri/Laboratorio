using System.Diagnostics;

namespace Core.Aplicacion.RespuestaUtilitario
{
    /// <summary>
    /// Clase generica de respuesta
    /// </summary>
    /// <typeparam name="T">Objeto respuesta</typeparam>
    public class Respuesta<T>
    {
        /// <summary>
        /// Si la petición fue exitosa
        /// </summary>
        public bool EsExitoso { get; set; }

        /// <summary>
        /// Mensaje de respuesta
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Lista de errores
        /// </summary>
        public List<string> Errores { get; set; }

        /// <summary>
        /// Objeto respuesta
        /// </summary>
        public T Dato { get; set; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Respuesta()
        {
            
        }

        /// <summary>
        /// Constructor de la clase cuando la respuesta es exitosa
        /// </summary>
        /// <param name="dato">Objeto respuesta</param>
        /// <param name="mensaje">Mensaje respuesta</param>
        public Respuesta(T dato, string mensaje = null)
        {
            EsExitoso = true;
            mensaje = mensaje;
            Dato = dato;
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="mensaje">Mensaje respuesta</param>
        public Respuesta(string mensaje)
        {
            EsExitoso = false;
            Mensaje = mensaje;
        }
    }
}
