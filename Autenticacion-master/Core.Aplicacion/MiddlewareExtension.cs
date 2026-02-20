using Azure.Core;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Comunes;
using Core.Dominio.Model;
using Core.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Polly;

namespace Core.Aplicacion
{
    public class MiddlewareExtension : IMiddleware
    {
        private readonly ILogger<MiddlewareExtension> _logger;
        readonly IRegistrarLog iRegistrarLog;
        private readonly ICacheServicio cacheServicio;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MiddlewareExtension(ILogger<MiddlewareExtension> logger, IRegistrarLog iRegistrarLog, ICacheServicio cacheServicio, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.iRegistrarLog = iRegistrarLog ?? throw new ArgumentException(nameof(iRegistrarLog));
            this.cacheServicio = cacheServicio;
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await next(context);

                responseBody.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();

                var endpoint = context.GetEndpoint() as RouteEndpoint;
                string? rawRoutePattern = endpoint?.RoutePattern.RawText;

                string usuarioContext = string.Empty;

                var idSesion = context.Request.Headers["IdSesion"].ToString();

                if (!string.IsNullOrEmpty(idSesion))
                {
                    var usuario = await cacheServicio.Obtener<UsuarioModel>(idSesion);
                    if (usuario != null)
                        usuarioContext = usuario.Usuario;
                }

                //await iRegistrarLog.RegistrarLog(rawRoutePattern, responseText, usuarioContext);

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"Response: {text}";
        }
    }
}