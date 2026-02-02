using System.Globalization;

namespace Core.Aplicacion.Errores
{
    public class ApiError : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ApiError() : base () { }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="mensaje">Mensaje error</param>
        public ApiError(string mensaje) : base(mensaje) { }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="mensaje">Mensaje error</param>
        public ApiError(string mensaje, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, mensaje, args)) { }

    }
}
