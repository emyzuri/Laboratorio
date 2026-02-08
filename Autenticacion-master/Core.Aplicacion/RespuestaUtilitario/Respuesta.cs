using System.Diagnostics;

namespace Core.Aplicacion.RespuestaUtilitario
{
    /// <summary>
    /// Clase generica de respuesta
    /// </summary>
    /// <typeparam name="T">Objeto respuesta</typeparam>
    public class Respuesta
    {
        private int codigo = 100000;

        private string mensaje = string.Empty;
        private IList<string>? errores ;

        /// <summary>
        /// Si la petición fue exitosa
        /// </summary>
        public bool EsExitoso { get; set; } = true;

        public object? Datos { get; set; }

        public int Codigo 
        {
            get { return codigo; }
            set { 
                if(value != 100000)
                {
                    EsExitoso = false;
                }
                codigo = value;
            }
        }

        public string Mensaje
        {
            get { return mensaje; }
            set
            {
                if (value != string.Empty && EsExitoso)
                {
                    EsExitoso = false;
                }

                if(!EsExitoso && codigo == 100000)
                {
                    codigo = 102999;
                }

                mensaje = value;
            }
        }

        public IList<string> Errores
        {
            get { return errores; }
            set
            {
                if (codigo < 0)
                {
                    errores = value;
                }
            }
        }

        /// <summary>
        /// Mensaje de respuesta
        /// </summary>
        //public string Mensaje { get; set; }

        /// <summary>
        /// Lista de errores
        /// </summary>
        //public List<string> Errores { get; set; }

        /// <summary>
        /// Objeto respuesta
        /// </summary>
        ////public T Dato { get; set; }

        ///// <summary>
        ///// Constructor de la clase
        ///// </summary>
        //public Respuesta()
        //{

        //}

        ///// <summary>
        ///// Constructor de la clase cuando la respuesta es exitosa
        ///// </summary>
        ///// <param name="dato">Objeto respuesta</param>
        ///// <param name="mensaje">Mensaje respuesta</param>
        //public Respuesta(T dato, string mensaje = null)
        //{
        //    EsExitoso = true;
        //    mensaje = mensaje;
        //    Dato = dato;
        //}

        ///// <summary>
        ///// Constructor de la clase
        ///// </summary>
        ///// <param name="mensaje">Mensaje respuesta</param>
        //public Respuesta(string mensaje)
        //{
        //    EsExitoso = false;
        //    Mensaje = mensaje;
        //}
    }
}
