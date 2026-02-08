using Core.Dominio.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.DataAccess.Menu.Interfaz
{
    public interface IMenu
    {
        Task<IEnumerable<MenuModel>> ObtenerMenu();
        Task<IEnumerable<MenuModel>> ObtenerMenus();
    }
}