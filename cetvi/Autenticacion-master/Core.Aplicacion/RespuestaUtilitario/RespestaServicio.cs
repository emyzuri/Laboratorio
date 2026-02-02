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
            try
            {
                Respuesta respuesta1 = respuesta;
                respuesta1.Datos = await metodo();
            }
            catch (Exception ex)
            {
                bitacora.LogError(ex, ex.Message);
                respuesta.Codigo = ex.HResult;
                respuesta.Mensaje = ex.Message;
                respuesta.Errores = new List<string> { ex.ToString() };
            }
            return respuesta;
        }
    }
}
