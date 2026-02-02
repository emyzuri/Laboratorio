using Core.Dominio.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.DataAccess.Menu.Interfaz
{
    public interface IMenu
    {
        Task<List<MenuModel>> ObtenerMenu();
        Task<List<MenuModel>> ObtenerMenus();
    }
}