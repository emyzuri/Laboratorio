using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.DataAccess.Menu.Interfaz;
using Core.Util;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace Core.Aplicacion.Funciones.Comandos.Menu
{
    public class ValidarMenuHandler : IRequestHandler<ValidarMenuCom, IEnumerable<MenuModel>>
    {
        private readonly IMenu iMenu;
        private readonly ICacheServicio cacheServicio;

        public ValidarMenuHandler(IMenu iMenu, ICacheServicio cacheServicio)
        {
            this.iMenu = iMenu ?? throw new ArgumentException(nameof(iMenu));
            this.cacheServicio = cacheServicio ?? throw new ArgumentException(nameof(cacheServicio));
        }

        public async Task<IEnumerable<MenuModel>> Handle(ValidarMenuCom request, CancellationToken cancellationToken)
        {
            IEnumerable<MenuModel> menu = await iMenu.ObtenerMenu();
            foreach (var item in menu)
            {
                var hijos = menu.Where(x => x.IdPadre == item.IdMenu).ToList();
                if (hijos.Any())
                {
                    item.SubMenus = hijos;
                }
            }
            return menu.Where(x => x.IdPadre == 0).ToList();

        }
    }
}