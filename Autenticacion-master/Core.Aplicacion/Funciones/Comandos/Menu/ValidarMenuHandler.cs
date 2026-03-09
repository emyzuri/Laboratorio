using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.DataAccess.Menu.Interfaz;
using Core.Dominio.Model;
using Core.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Polly;
using System.Collections.Generic;
namespace Core.Aplicacion.Funciones.Comandos.Menu
{
    public class ValidarMenuHandler : IRequestHandler<ValidarMenuCom, IEnumerable<MenuModel>>
    {
        private readonly IMenu iMenu;
        private readonly ICacheServicio cacheServicio;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ValidarMenuHandler(IMenu iMenu, ICacheServicio cacheServicio, IHttpContextAccessor httpContextAccessor)
        {
            this.iMenu = iMenu ?? throw new ArgumentException(nameof(iMenu));
            this.cacheServicio = cacheServicio ?? throw new ArgumentException(nameof(cacheServicio));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<MenuModel>> Handle(ValidarMenuCom request, CancellationToken cancellationToken)
        {
            var idSesion = httpContextAccessor.HttpContext.Request.Headers["IdSesion"].ToString();
            UsuarioModel usuario = await cacheServicio.Obtener<UsuarioModel>(idSesion);
            if (usuario == null)
            {
                return new List<MenuModel>();
            }
            IEnumerable<MenuModel> menu = await iMenu.ObtenerMenu();
            IEnumerable<MenuModel> rolMenu = await iMenu.ObtenerMenusPorRol(usuario.IdUsuario);
            var menusPermitidos = menu.Where(a => rolMenu.Any(b => b.IdMenu == a.IdMenu)).ToList();

            foreach (var item in menusPermitidos)
            {
                var hijos = menusPermitidos.Where(x => x.IdPadre == item.IdMenu).ToList();
                if (hijos.Any())
                {
                    item.SubMenus = hijos;
                }
            }
            return menusPermitidos.Where(x => x.IdPadre == 0).ToList();
        }
    }
}