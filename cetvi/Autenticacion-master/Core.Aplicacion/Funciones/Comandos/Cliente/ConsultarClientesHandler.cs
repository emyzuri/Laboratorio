using Core.Aplicacion.Funciones.Comandos.Usuario;
using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Aplicacion.Funciones.Comandos.Cliente
{
    internal class ConsultarClientesHandler : IRequestHandler<ConsultarClientesCom, List<UsuarioModel>>
    {
        /// <summary>
        /// Servicio de cliente
        /// </summary>
        readonly IUsuario iUsuario;
        readonly ICacheServicio cacheServicio;

        readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="iUsuario">Servicio de cliente</param>
        /// <exception cref="ArgumentException">Control de errores</exception>
        public ConsultarClientesHandler(IUsuario iUsuario, ICacheServicio cacheServicio, IHttpContextAccessor httpContextAccessor)
        {
            this.iUsuario = iUsuario ?? throw new ArgumentException(nameof(iUsuario));
            this.cacheServicio = cacheServicio ?? throw new ArgumentException(nameof(cacheServicio));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// Logica de consula del cliente
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Cliente</returns>
        /// <exception cref="NotImplementedException">Control de errores</exception>
        public async Task<List<UsuarioModel>> Handle(ConsultarClientesCom request, CancellationToken cancellationToken)
        {
            UsuarioModel usuario = await cacheServicio.Obtener<UsuarioModel>(httpContextAccessor.HttpContext.Request.Headers["IdSesion"]);
            if (usuario == null)
            {
                throw new ArgumentException("Sesión caducada");
            }
            return await iUsuario.ObtenerUsuarios();
        }
    }
}