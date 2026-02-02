using Core.DataAccess.Clientes.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Pipelines.Sockets.Unofficial.Arenas;

namespace Core.Aplicacion.Funciones.Comandos.Usuario
{
    public class ValidarUsuarioHandler : IRequestHandler<ValidarUsuarioCom, UsuarioModel>
    {
        /// <summary>
        /// Servicio de cliente
        /// </summary>
        readonly IUsuario iUsuario;
        readonly ICacheServicio cacheServicio;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="iUsuario">Servicio de cliente</param>
        /// <exception cref="ArgumentException">Control de errores</exception>
        public ValidarUsuarioHandler(IUsuario iUsuario, ICacheServicio cacheServicio)
        {
            this.iUsuario = iUsuario ?? throw new ArgumentException(nameof(iUsuario));
            this.cacheServicio = cacheServicio ?? throw new ArgumentException(nameof(cacheServicio));
        }

        /// <summary>
        /// Logica de consula del cliente
        /// </summary>
        /// <param name="request">Objeto transaccional</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>Cliente</returns>
        /// <exception cref="NotImplementedException">Control de errores</exception>
        public async Task<UsuarioModel> Handle(ValidarUsuarioCom request, CancellationToken cancellationToken)
        {
            UsuarioModel usuario = await iUsuario.ObtenerUsuario(request.usuario, request.password);
            await cacheServicio.Agregar(usuario.IdSesion.ToString(), usuario, new TimeSpan(0,6,0));
            return usuario;
        }
    }
}
