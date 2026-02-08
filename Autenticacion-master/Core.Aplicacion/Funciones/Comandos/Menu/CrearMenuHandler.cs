using Core.Util;
using MediatR;
using Core.Aplicacion.RespuestaUtilitario;
using Core.Aplicacion.Funciones.Comandos.Cliente;
using Core.DataAccess.Menu.Interfaz;

namespace Core.Aplicacion.Funciones.Comandos.Menu
{
    public class CrearMenuHandler : IRequestHandler<CrearMenuCom, int>
    {
        private readonly IMenu iMenu;
        private readonly ICacheServicio cacheServicio;

        public CrearMenuHandler(IMenu iMenu, ICacheServicio cacheServicio)
        {
            this.iMenu = iMenu;
            this.cacheServicio = cacheServicio;
        }

        public async Task<int> Handle(CrearMenuCom request, CancellationToken cancellationToken)
        {
            await cacheServicio.Agregar("menu_temp", request.Nombre, new TimeSpan(0, 2, 0));
            return 10;
        }
    }
}