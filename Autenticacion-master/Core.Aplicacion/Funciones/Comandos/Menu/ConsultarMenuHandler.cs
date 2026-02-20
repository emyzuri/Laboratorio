using Core.Aplicacion.Funciones.Comandos.Usuarios;
using Core.DataAccess.Menu.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Aplicacion.Funciones.Comandos.Menu
{
    public class ConsultarMenuHandler : IRequestHandler<ConsultarMenuCom, List<MenuModel>>
    {
        private readonly IMenu iMenu;
        private readonly ICacheServicio cacheServicio;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ConsultarMenuHandler(IMenu iMenu, ICacheServicio cacheServicio, IHttpContextAccessor httpContextAccessor)
        {
            this.iMenu = iMenu ?? throw new ArgumentNullException(nameof(iMenu));
            this.cacheServicio = cacheServicio ?? throw new ArgumentNullException(nameof(cacheServicio));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<List<MenuModel>> Handle(ConsultarMenuCom request, CancellationToken cancellationToken)
        {
            UsuarioModel usuario = await cacheServicio.Obtener<UsuarioModel>(httpContextAccessor.HttpContext.Request.Headers["IdSesion"]);

            if (usuario == null)
            {
                throw new ArgumentException("Sesión caducada");
            }
            var todosLosMenus = (await iMenu.ObtenerMenus()).ToList();
            if (usuario.IdRol == 2)
            {
                todosLosMenus = todosLosMenus.Where(m =>
                    !m.Nombre.Contains("Usuario", StringComparison.OrdinalIgnoreCase) &&
                    !(m.Url ?? "").Contains("usuario", StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            var listaJerarquica = todosLosMenus.Where(m => m.IdPadre == 0).ToList();

            foreach (var padre in listaJerarquica)
            {
                padre.SubMenus = todosLosMenus.Where(m => m.IdPadre == padre.IdMenu).ToList();
            }

            return listaJerarquica;
        }
    }
}