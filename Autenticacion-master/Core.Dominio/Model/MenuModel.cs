using System;
using System.Collections.Generic;

namespace Core.Dominio.Model
{
    public class MenuModel
    {
        public int IdMenu { get; set; }
        public int IdPadre { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public string Icono { get; set; }
        public List<MenuModel> SubMenus { get; set; }
        public MenuModel()
        {
            SubMenus = new List<MenuModel>();
        }
    }
}