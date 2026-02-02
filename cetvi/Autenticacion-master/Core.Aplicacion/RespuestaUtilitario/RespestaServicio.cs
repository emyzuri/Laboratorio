using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aplicacion.RespuestaUtilitario
{
    public static class RespestaServicio
    {
        private const string CODIGO_HOMOLOGACION = "";

        public static async Task<Respuesta> CrearRespuestaExito<TResult>(ILogger bitacora, Func<Task<TResult>> metodo)
        {
            Respuesta respuesta = new Respuesta();
            return new Respuesta
            {
                EsExitoso = true,
                Datos = datos,
                Codigo = 0,
                Mensaje = "Operación exitosa"
            };
        }
    }
}
